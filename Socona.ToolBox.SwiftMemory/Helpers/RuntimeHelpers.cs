using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.SwiftMemory.Helpers
{

    /// <summary>
    /// A helper class that with utility methods for dealing with references, and other low-level details.
    /// It also contains some APIs that act as polyfills for .NET Standard 2.0 and below.
    /// </summary>
    internal static class RuntimeHelpers
    {
        /// <summary>
        /// Converts a length of items from one size to another (rounding towards zero).
        /// </summary>
        /// <typeparam name="TFrom">The source type of items.</typeparam>
        /// <typeparam name="TTo">The target type of items.</typeparam>
        /// <param name="length">The input length to convert.</param>
        /// <returns>The converted length for the specified argument and types.</returns>
     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int ConvertLength<TFrom, TTo>(int length)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            if (sizeof(TFrom) == sizeof(TTo))
            {
                return length;
            }
            else if (sizeof(TFrom) == 1)
            {
                return (int)((uint)length / (uint)sizeof(TTo));
            }
            else
            {
                ulong targetLength = (ulong)(uint)length * (uint)sizeof(TFrom) / (uint)sizeof(TTo);

                return checked((int)targetLength);
            }
        }

        /// <summary>
        /// Gets the length of a given array as a native integer.
        /// </summary>
        /// <typeparam name="T">The type of values in the array.</typeparam>
        /// <param name="array">The input <see cref="Array"/> instance.</param>
        /// <returns>The total length of <paramref name="array"/> as a native integer.</returns>
        /// <remarks>
        /// This method is needed because this expression is not inlined correctly if the target array
        /// is only visible as a non-generic <see cref="Array"/> instance, because the C# compiler will
        /// not be able to emit the <see langword="ldlen"/> opcode instead of calling the right method.
        /// </remarks>
      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint GetArrayNativeLength<T>(T[] array)
        {

            return (nint)array.LongLength;
        }

        /// <summary>
        /// Gets the length of a given array as a native integer.
        /// </summary>
        /// <param name="array">The input <see cref="Array"/> instance.</param>
        /// <returns>The total length of <paramref name="array"/> as a native integer.</returns>
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint GetArrayNativeLength(Array array)
        {

            return (nint)array.LongLength;

        }

        /// <summary>
        /// Gets the byte offset to the first <typeparamref name="T"/> element in a SZ array.
        /// </summary>
        /// <typeparam name="T">The type of values in the array.</typeparam>
        /// <returns>The byte offset to the first <typeparamref name="T"/> element in a SZ array.</returns>
      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr GetArrayDataByteOffset<T>()
        {
            return TypeInfo<T>.ArrayDataByteOffset;
        }

        /// <summary>
        /// Gets the byte offset to the first <typeparamref name="T"/> element in a 2D array.
        /// </summary>
        /// <typeparam name="T">The type of values in the array.</typeparam>
        /// <returns>The byte offset to the first <typeparamref name="T"/> element in a 2D array.</returns>
     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr GetArray2DDataByteOffset<T>()
        {
            return TypeInfo<T>.Array2DDataByteOffset;
        }

        /// <summary>
        /// Gets the byte offset to the first <typeparamref name="T"/> element in a 3D array.
        /// </summary>
        /// <typeparam name="T">The type of values in the array.</typeparam>
        /// <returns>The byte offset to the first <typeparamref name="T"/> element in a 3D array.</returns>
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr GetArray3DDataByteOffset<T>()
        {
            return TypeInfo<T>.Array3DDataByteOffset;
        }


        /// <summary>
        /// A private generic class to preload type info for arbitrary runtime types.
        /// </summary>
        /// <typeparam name="T">The type to load info for.</typeparam>
        private static class TypeInfo<T>
        {
            /// <summary>
            /// The byte offset to the first <typeparamref name="T"/> element in a SZ array.
            /// </summary>
            public static readonly IntPtr ArrayDataByteOffset = MeasureArrayDataByteOffset();

            /// <summary>
            /// The byte offset to the first <typeparamref name="T"/> element in a 2D array.
            /// </summary>
            public static readonly IntPtr Array2DDataByteOffset = MeasureArray2DDataByteOffset();

            /// <summary>
            /// The byte offset to the first <typeparamref name="T"/> element in a 3D array.
            /// </summary>
            public static readonly IntPtr Array3DDataByteOffset = MeasureArray3DDataByteOffset();



            /// <summary>
            /// Computes the value for <see cref="ArrayDataByteOffset"/>.
            /// </summary>
            /// <returns>The value of <see cref="ArrayDataByteOffset"/> for the current runtime.</returns>
           
            private static IntPtr MeasureArrayDataByteOffset()
            {
                var array = new T[1];

                return ObjectMarshal.DangerousGetObjectDataByteOffset(array, ref array[0]);
            }

            /// <summary>
            /// Computes the value for <see cref="Array2DDataByteOffset"/>.
            /// </summary>
            /// <returns>The value of <see cref="Array2DDataByteOffset"/> for the current runtime.</returns>
          
            private static IntPtr MeasureArray2DDataByteOffset()
            {
                var array = new T[1, 1];

                return ObjectMarshal.DangerousGetObjectDataByteOffset(array, ref array[0, 0]);
            }

            /// <summary>
            /// Computes the value for <see cref="Array3DDataByteOffset"/>.
            /// </summary>
            /// <returns>The value of <see cref="Array3DDataByteOffset"/> for the current runtime.</returns>
       
            private static IntPtr MeasureArray3DDataByteOffset()
            {
                var array = new T[1, 1, 1];

                return ObjectMarshal.DangerousGetObjectDataByteOffset(array, ref array[0, 0, 0]);
            }
        }
    }
}
