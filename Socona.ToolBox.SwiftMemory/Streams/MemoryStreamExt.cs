using Socona.ToolBox.SwiftMemory.Sources;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Streams
{
    internal class MemoryStreamExt
    {
        /// <summary>
        /// Creates a new <see cref="Stream"/> from the input <see cref="ReadOnlyMemory{T}"/> of <see cref="byte"/> instance.
        /// </summary>
        /// <param name="memory">The input <see cref="ReadOnlyMemory{T}"/> instance.</param>
        /// <param name="isReadOnly">Indicates whether or not <paramref name="memory"/> can be written to.</param>
        /// <returns>A <see cref="Stream"/> wrapping the underlying data for <paramref name="memory"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="memory"/> has an invalid data store.</exception>
        
        public static Stream Create(ReadOnlyMemory<byte> memory, bool isReadOnly)
        {
            if (memory.IsEmpty)
            {
                // Return an empty stream if the memory was empty
                return new MemoryStream<ArrayOwner>(ArrayOwner.Empty, isReadOnly);
            }

            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                var arraySpanSource = new ArrayOwner(segment.Array!, segment.Offset, segment.Count);

                return new MemoryStream<ArrayOwner>(arraySpanSource, isReadOnly);
            }

            if (MemoryMarshal.TryGetMemoryManager<byte, MemoryManager<byte>>(memory, out var memoryManager, out int start, out int length))
            {
                MemoryManagerOwner memoryManagerSpanSource = new MemoryManagerOwner(memoryManager, start, length);

                return new MemoryStream<MemoryManagerOwner>(memoryManagerSpanSource, isReadOnly);
            }

            return ThrowNotSupportedExceptionForInvalidMemory();
        }

        /// <summary>
        /// Creates a new <see cref="Stream"/> from the input <see cref="IMemoryOwner{T}"/> of <see cref="byte"/> instance.
        /// </summary>
        /// <param name="memoryOwner">The input <see cref="IMemoryOwner{T}"/> instance.</param>
        /// <returns>A <see cref="Stream"/> wrapping the underlying data for <paramref name="memoryOwner"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="memoryOwner"/> has an invalid data store.</exception>
      
        public static Stream Create(IMemoryOwner<byte> memoryOwner)
        {
            Memory<byte> memory = memoryOwner.Memory;

            if (memory.IsEmpty)
            {
                // Return an empty stream if the memory was empty
                return new IMemoryOwnerStream<ArrayOwner>(ArrayOwner.Empty, memoryOwner);
            }

            if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
            {
                var arraySpanSource = new ArrayOwner(segment.Array!, segment.Offset, segment.Count);

                return new IMemoryOwnerStream<ArrayOwner>(arraySpanSource, memoryOwner);
            }

            if (MemoryMarshal.TryGetMemoryManager<byte, MemoryManager<byte>>(memory, out var memoryManager, out int start, out int length))
            {
                MemoryManagerOwner memoryManagerSpanSource = new MemoryManagerOwner(memoryManager, start, length);

                return new IMemoryOwnerStream<MemoryManagerOwner>(memoryManagerSpanSource, memoryOwner);
            }

            return ThrowNotSupportedExceptionForInvalidMemory();
        }

        /// <summary>
        /// Throws a <see cref="ArgumentException"/> when a given <see cref="Memory{T}"/>
        /// or <see cref="IMemoryOwner{T}"/> instance has an unsupported backing store.
        /// </summary>
        /// <returns>Nothing, this method always throws.</returns>
        private static Stream ThrowNotSupportedExceptionForInvalidMemory()
        {
            throw new ArgumentException("The input instance doesn't have a valid underlying data store.");
        }
    }
}
