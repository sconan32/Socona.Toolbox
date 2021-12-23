using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox
{
    public class NullType<T>
    {
        //Nothing Here
        private NullType()
        {
        }
        public static NullType<T> Instance { get; } = new NullType<T>();
    }
}
