﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Buffers
{
    /// <summary>
    /// A debug proxy used to display items in a 1D layout.
    /// </summary>
    /// <typeparam name="T">The type of items to display.</typeparam>
    internal sealed class MemoryDebugView<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="arrayPoolBufferWriter">The input <see cref="ArrayPoolBufferWriter{T}"/> instance with the items to display.</param>
        public MemoryDebugView(ArrayPoolBufferWriter<T>? arrayPoolBufferWriter)
        {
            this.Items = arrayPoolBufferWriter?.WrittenSpan.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="memoryBufferWriter">The input <see cref="MemoryBufferWriter{T}"/> instance with the items to display.</param>
        public MemoryDebugView(MemoryBufferWriter<T>? memoryBufferWriter)
        {
            this.Items = memoryBufferWriter?.WrittenSpan.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="memoryOwner">The input <see cref="MemoryOwner{T}"/> instance with the items to display.</param>
        public MemoryDebugView(MemoryOwner<T>? memoryOwner)
        {
            this.Items = memoryOwner?.Span.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="spanOwner">The input <see cref="SpanOwner{T}"/> instance with the items to display.</param>
        public MemoryDebugView(SpanOwner<T> spanOwner)
        {
            this.Items = spanOwner.Span.ToArray();
        }

        /// <summary>
        /// Gets the items to display for the current instance
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public T[]? Items { get; }
    }
}
