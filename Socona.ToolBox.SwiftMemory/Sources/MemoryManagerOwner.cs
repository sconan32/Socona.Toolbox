using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Sources
{
    /// <summary>
    /// An <see cref="ISpanOwner"/> implementation wrapping a <see cref="MemoryManager{T}"/> of <see cref="byte"/> instance.
    /// </summary>
    internal readonly struct MemoryManagerOwner : ISpanOwner
    {
        /// <summary>
        /// The wrapped <see cref="MemoryManager{T}"/> instance.
        /// </summary>
        private readonly MemoryManager<byte> memoryManager;

        /// <summary>
        /// The starting offset within <see cref="memoryManager"/>.
        /// </summary>
        private readonly int offset;

        /// <summary>
        /// The usable length within <see cref="memoryManager"/>.
        /// </summary>
        private readonly int length;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryManagerOwner"/> struct.
        /// </summary>
        /// <param name="memoryManager">The wrapped <see cref="MemoryManager{T}"/> instance.</param>
        /// <param name="offset">The starting offset within <paramref name="memoryManager"/>.</param>
        /// <param name="length">The usable length within <paramref name="memoryManager"/>.</param>
        public MemoryManagerOwner(MemoryManager<byte> memoryManager, int offset, int length)
        {
            this.memoryManager = memoryManager;
            this.offset = offset;
            this.length = length;
        }

        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.length;
        }

        /// <inheritdoc/>
        public Span<byte> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // We can't use the same trick we use for arrays to optimize the creation of
                // the offset span, as otherwise a bugged MemoryManager<T> instance returning
                // a span of an incorrect size could cause an access violation. Instead, we just
                // get the span and then slice it, which will validate both offset and length.
                return this.memoryManager.GetSpan().Slice(this.offset, this.length);
            }
        }
    }
}
