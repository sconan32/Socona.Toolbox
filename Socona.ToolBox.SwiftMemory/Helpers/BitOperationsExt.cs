using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Helpers
{
    /// <summary>
    /// Utility methods for intrinsic bit-twiddling operations. The methods use hardware intrinsics
    /// when available on the underlying platform, otherwise they use optimized software fallbacks.
    /// </summary>
    internal class BitOperationsExt
    {
        /// <summary>
        /// Rounds up an <see cref="int"/> value to a power of 2.
        /// </summary>
        /// <param name="x">The input value to round up.</param>
        /// <returns>The smallest power of two greater than or equal to <paramref name="x"/>.</returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundUpPowerOfTwo(int x)
        {
            return 1 << (32 - BitOperations.LeadingZeroCount((uint)(x - 1)));
        }
    }
}
