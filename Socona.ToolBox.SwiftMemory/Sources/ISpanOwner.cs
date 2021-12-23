using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Sources
{

    /// <summary>
    /// An interface for types acting as sources for <see cref="Span{T}"/> instances.
    /// </summary>
    internal interface ISpanOwner
    {
        /// <summary>
        /// Gets the length of the underlying memory area.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets a <see cref="Span{T}"/> instance wrapping the underlying memory area.
        /// </summary>
        Span<byte> Span { get; }
    }
}
