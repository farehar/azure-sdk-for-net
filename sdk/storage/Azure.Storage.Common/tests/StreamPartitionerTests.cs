﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Testing;
using NUnit.Framework;

namespace Azure.Storage.Test
{
    [TestFixture]
    public class StreamPartitionerTests
    {
        [Test]
        public async Task GetNextPartitionAsync()
        {
            var expected = TestHelper.GetRandomBuffer(10 * Constants.MB);
            var actual = new byte[expected.Length];

            Assert.AreNotSame(expected, actual);

            using (var expectedStream = new NonSeekableStream(expected))
            using (var reader = new StreamPartitioner(expectedStream))
            {
                Assert.IsTrue(expectedStream.CanRead);
                Assert.IsFalse(expectedStream.CanSeek);

                do
                {
                    var position = expectedStream.Position;
                    using (StreamPartition buffer = await reader.GetNextPartitionAsync())
                    {
                        if (buffer.Length == 0)
                        {
                            Assert.AreEqual(expectedStream.Length, expectedStream.Position);
                            break;
                        }
                        else
                        {
                            Assert.IsTrue(buffer.CanRead);
                            Assert.IsTrue(buffer.CanSeek);

                            await buffer.ReadAsync(actual, (int)position, (int)buffer.Length);
                        }
                    }
                }
                while (true);

                TestHelper.AssertSequenceEqual(expected, actual);
            }
        }

        [Test]
        public async Task Read_WithReadOnlyMemory()
        {
            var expected = TestHelper.GetRandomBuffer(10 * Constants.MB);
            var actual = new byte[expected.Length];

            Assert.AreNotSame(expected, actual);

            using (var expectedStream = new NonSeekableStream(expected))
            using (var reader = new StreamPartitioner(expectedStream))
            {
                Assert.IsTrue(expectedStream.CanRead);
                Assert.IsFalse(expectedStream.CanSeek);

                do
                {
                    var position = expectedStream.Position;
                    using (StreamPartition buffer = await reader.GetNextPartitionAsync())
                    {
                        if (buffer.Length == 0)
                        {
                            Assert.AreEqual(expectedStream.Length, expectedStream.Position);
                            break;
                        }
                        else
                        {
                            Assert.IsTrue(buffer.CanRead);
                            Assert.IsTrue(buffer.CanSeek);

                            buffer.Read(out ReadOnlyMemory<byte> memory, (int)buffer.Length);

                            memory.CopyTo(new Memory<byte>(actual, (int)position, (int)buffer.Length));
                        }
                    }
                }
                while (true);

                TestHelper.AssertSequenceEqual(expected, actual);
            }
        }

        [Test]
        [NonParallelizable]
        public async Task GetNextPartitionAsync_1GB()
        {
            long memoryStart;
            long memoryEnd;
            var buffersRead = 0L;

            var length = (1L * Constants.GB) - (1 * Constants.MB) - (1 * Constants.KB);

            using (var expectedStream = new MockNonSeekableStream(length))
            using (var reader = new StreamPartitioner(expectedStream))
            {
                memoryStart = GC.GetTotalMemory(true);

                Assert.IsTrue(expectedStream.CanRead);
                Assert.IsFalse(expectedStream.CanSeek);

                do
                {
                    var position = expectedStream.Position;
                    using (StreamPartition buffer = await reader.GetNextPartitionAsync())
                    {
                        if (buffer.Length == 0)
                        {
                            Assert.AreEqual(expectedStream.Length, expectedStream.Position);
                            break;
                        }
                        else
                        {
                            buffersRead++;

                            Assert.IsTrue(buffer.CanRead);
                            Assert.IsTrue(buffer.CanSeek);
                        }
                    }

                    Assert.IsTrue(GC.GetTotalMemory(true) - memoryStart < 8 * Storage.Constants.DefaultBufferSize); // TODO Assuming at most 8 buffers allocated
                }
                while (true);
            }

            memoryEnd = GC.GetTotalMemory(true);

            //logger.LogInformation($"{buffersRead} buffers read");
            //logger.LogInformation($"{nameof(memoryStart)} = {memoryStart}; {nameof(memoryEnd)} = {memoryEnd}");
            //logger.LogInformation($"delta = {memoryEnd - memoryStart}");

            Assert.AreEqual(Math.Ceiling(1d * length / Storage.Constants.DefaultBufferSize), buffersRead);
            Assert.IsTrue(memoryEnd - memoryStart < 8 * Storage.Constants.DefaultBufferSize); // TODO Assuming at most 8 buffers allocated
        }

        [Test]
        [LiveOnly]
        [NonParallelizable]
        public async Task GetNextPartitionAsync_1GB_WithReadOnlyMemory()
        {
            long memoryStart;
            long memoryEnd;
            var buffersRead = 0L;

            var length = (1L * Constants.GB) - (1 * Constants.MB) - (1 * Constants.KB);

            using (var expectedStream = new MockNonSeekableStream(length))
            using (var reader = new StreamPartitioner(expectedStream))
            {
                memoryStart = GC.GetTotalMemory(true);

                Assert.IsTrue(expectedStream.CanRead);
                Assert.IsFalse(expectedStream.CanSeek);

                do
                {
                    var position = expectedStream.Position;
                    using (StreamPartition buffer = await reader.GetNextPartitionAsync())
                    {
                        if (buffer.Length == 0)
                        {
                            Assert.AreEqual(expectedStream.Length, expectedStream.Position);
                            break;
                        }
                        else
                        {
                            buffersRead++;

                            Assert.IsTrue(buffer.CanRead);
                            Assert.IsTrue(buffer.CanSeek);

                            buffer.Read(out ReadOnlyMemory<byte> memory, (int)buffer.Length);

                            Assert.AreEqual((int)buffer.Length, memory.Length);
                        }
                    }

                    Assert.IsTrue(GC.GetTotalMemory(true) - memoryStart < 8 * Storage.Constants.DefaultBufferSize); // TODO Assuming at most 8 buffers allocated
                }
                while (true);
            }

            memoryEnd = GC.GetTotalMemory(true);

            //logger.LogInformation($"{buffersRead} buffers read");
            //logger.LogInformation($"{nameof(memoryStart)} = {memoryStart}; {nameof(memoryEnd)} = {memoryEnd}");
            //logger.LogInformation($"delta = {memoryEnd - memoryStart}");

            Assert.AreEqual(Math.Ceiling(1d * length / Storage.Constants.DefaultBufferSize), buffersRead);
            Assert.IsTrue(memoryEnd - memoryStart < 8 * Storage.Constants.DefaultBufferSize); // TODO Assuming at most 8 buffers allocated
        }

        [Test]
        [LiveOnly]
        [NonParallelizable]
        public async Task GetNextPartitionAsync_1GB_Sequencing()
        {
            var length = (511 * Constants.MB) - (1 * Constants.KB);
            var numberOfBytesToSample = 1 * Constants.KB;
            var maxActivePartitions = 4;
            var maxLoadedPartitions = 8;

            var expected = TestHelper.GetRandomBuffer(length);
            var actual = new byte[expected.Length];

            Assert.AreNotSame(expected, actual);

            var sequence = new List<(long position, ReadOnlyMemory<byte> bytes)>();

            using (var expectedStream = new NonSeekableStream(expected))
            using (var reader = new StreamPartitioner(expectedStream))
            {
                Assert.IsTrue(expectedStream.CanRead);
                Assert.IsFalse(expectedStream.CanSeek);

                await foreach (StreamPartition buffer in reader.GetPartitionsAsync(maxActivePartitions, maxLoadedPartitions))
                {
                    using (buffer)
                    {
                        if (buffer.Length == 0)
                        {
                            Assert.AreEqual(expectedStream.Length, expectedStream.Position);
                            break;
                        }
                        else
                        {
                            buffer.Read(out ReadOnlyMemory<byte> memory, numberOfBytesToSample);

                            sequence.Add((buffer.ParentPosition, memory));
                        }
                    }
                }
            }

            var currentPosition = 0L;

            foreach ((long position, ReadOnlyMemory<byte> bytes) buffer in sequence.Skip(1))
            {
                Assert.IsTrue(currentPosition < buffer.position, "Partitions received out of order");
                var expectedBytes = new byte[buffer.bytes.Length];
                Array.Copy(expected, buffer.position, expectedBytes, 0, buffer.bytes.Length);

                TestHelper.AssertSequenceEqual(expectedBytes, buffer.bytes.ToArray());

                currentPosition = buffer.position;
            }
        }

        private class NonSeekableStream : MemoryStream
        {
            public NonSeekableStream(byte[] buffer) : base(buffer)
            {
            }

            public override bool CanSeek => false;

            public override long Position
            {
                get => base.Position;
                set => throw new InvalidOperationException();
            }

            public override long Seek(long offset, SeekOrigin loc) => throw new InvalidOperationException();
        }

        private class MockNonSeekableStream : Stream
        {
            private static int s_seed = Environment.TickCount;
            private static readonly ThreadLocal<Random> s_random =
                new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref s_seed)));

            public static Random Random => s_random.Value;

            public MockNonSeekableStream(long length, bool randomizeData = false)
            {
                Length = length;
                _position = 0;
                _randomizeData = randomizeData;
            }

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => false;

            public override long Length { get; }

            private long _position;
            private readonly bool _randomizeData;

            public override long Position
            {
                get => _position;
                set => throw new InvalidOperationException();
            }

            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (_randomizeData)
                {
                    lock (this)
                    {
                        var i = 0;

                        for (i = 0; i < count && _position < Length; i++)
                        {
                            buffer[offset + i] = (byte)Random.Next(256);
                            Interlocked.Increment(ref _position);
                        }

                        return i;
                    }
                }
                else
                {
                    lock (this)
                    {
                        var i = (int)Math.Min(count, Length - _position);

                        Interlocked.Add(ref _position, i);

                        return i;
                    }
                }
            }

            public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException();

            public override void SetLength(long value) => throw new InvalidOperationException();

            public override void Write(byte[] buffer, int offset, int count) => throw new InvalidOperationException();
        }
    }
}
