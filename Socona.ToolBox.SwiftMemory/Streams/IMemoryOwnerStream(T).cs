using Socona.ToolBox.SwiftMemory.Buffers;
using Socona.ToolBox.SwiftMemory.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Streams
{
    /// <summary>
    /// A <see cref="Stream"/> implementation wrapping an <see cref="IMemoryOwner{T}"/> of <see cref="byte"/> instance.
    /// </summary>
    /// <typeparam name="TSource">The type of source to use for the underlying data.</typeparam>
    internal sealed class IMemoryOwnerStream<TSource> : MemoryStream<TSource>
        where TSource : struct, ISpanOwner
    {
        /// <summary>
        /// The <see cref="IDisposable"/> instance currently in use.
        /// </summary>
        private readonly IDisposable disposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMemoryOwnerStream{TSource}"/> class.
        /// </summary>
        /// <param name="source">The input <typeparamref name="TSource"/> instance to use.</param>
        /// <param name="disposable">The <see cref="IDisposable"/> instance currently in use.</param>
        public IMemoryOwnerStream(TSource source, IDisposable disposable)
            : base(source, false)
        {
            this.disposable = disposable;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.disposable.Dispose();
        }
    }
}
