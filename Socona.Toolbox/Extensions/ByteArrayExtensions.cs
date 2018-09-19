using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64String(this byte[] array)
        {
            if (array != null)
            {
                return Convert.ToBase64String(array);
            }
            return null;

        }
    }
}
