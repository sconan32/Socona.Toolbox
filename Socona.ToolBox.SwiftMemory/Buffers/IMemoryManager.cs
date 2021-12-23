using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Buffers
{
    /// <summary>
    /// An interface for a <see cref="MemoryManager{T}"/> instance that can reinterpret its underlying data.
    /// </summary>
    internal interface IMemoryManager
    {
        /// <summary>
        /// Creates a new <see cref="Memory{T}"/> that reinterprets the underlying data for the current instance.
        /// </summary>
        /// <typeparam name="T">The target type to cast the items to.</typeparam>
        /// <param name="offset">The starting offset within the data store.</param>
        /// <param name="length">The original used length for the data store.</param>
        /// <returns>A new <see cref="Memory{T}"/> instance of the specified type, reinterpreting the current items.</returns>
        Memory<T> GetMemory<T>(int offset, int length)
            where T : unmanaged;
    }
}
