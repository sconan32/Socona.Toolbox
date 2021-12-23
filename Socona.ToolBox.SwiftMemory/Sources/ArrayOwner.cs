using Socona.ToolBox.SwiftMemory.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Sources
{
    /// <summary>
    /// An <see cref="ISpanOwner"/> implementation wrapping an array.
    /// </summary>
    internal readonly struct ArrayOwner : ISpanOwner
    {
        /// <summary>
        /// The wrapped <see cref="byte"/> array.
        /// </summary>
        private readonly byte[] array;

        /// <summary>
        /// The starting offset within <see cref="array"/>.
        /// </summary>
        private readonly int offset;

        /// <summary>
        /// The usable length within <see cref="array"/>.
        /// </summary>
        private readonly int length;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayOwner"/> struct.
        /// </summary>
        /// <param name="array">The wrapped <see cref="byte"/> array.</param>
        /// <param name="offset">The starting offset within <paramref name="array"/>.</param>
        /// <param name="length">The usable length within <paramref name="array"/>.</param>
        public ArrayOwner(byte[] array, int offset, int length)
        {
            this.array = array;
            this.offset = offset;
            this.length = length;
        }

        /// <summary>
        /// Gets an empty <see cref="ArrayOwner"/> instance.
        /// </summary>
        public static ArrayOwner Empty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Array.Empty<byte>(), 0, 0);
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
                ref byte r0 = ref this.array.DangerousGetReferenceAt(this.offset);
                return MemoryMarshal.CreateSpan(ref r0, this.length);

            }
        }
    }
}
