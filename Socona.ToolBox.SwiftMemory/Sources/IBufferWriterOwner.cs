﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Sources
{
    /// <summary>
    /// An <see cref="IBufferWriter{T}"/> implementation wrapping an <see cref="IBufferWriter{T}"/> instance.
    /// </summary>
    internal readonly struct IBufferWriterOwner : IBufferWriter<byte>
    {
        /// <summary>
        /// The wrapped <see cref="ArrayPoolBufferWriter{T}"/> array.
        /// </summary>
        private readonly IBufferWriter<byte> writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IBufferWriterOwner"/> struct.
        /// </summary>
        /// <param name="writer">The wrapped <see cref="IBufferWriter{T}"/> instance.</param>
        public IBufferWriterOwner(IBufferWriter<byte> writer)
        {
            this.writer = writer;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            this.writer.Advance(count);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            return this.writer.GetMemory(sizeHint);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            return this.writer.GetSpan(sizeHint);
        }
    }
}
