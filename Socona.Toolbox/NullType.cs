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
        public static readonly NullType<T> Instance = new NullType<T>();
    }
}
