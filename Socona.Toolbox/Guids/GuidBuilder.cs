using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Socona.ToolBox.Guids
{
    public class GuidBuilder
    {
        private GuidBuilder()
        { }

       public static GuidBuilder Instance = new GuidBuilder();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid Build(int d)
        {
            return Build(0, 0, 0, d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid Build(int a, int d)
        {
            return Build(a, 0, 0, d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Guid Build(int a, int b, int c, int d)
        {
            short b1 = (short)b;
            short b2 = (short)(b >> 16);

            short c1 = (short)c;
            short c2 = (short)(c >> 16);
            return new Guid($"{a:X8}-{b1:X4}-{b2:X4}-{c1:X4}-{c2:X4}{d:X8}");
        }



    }
}
