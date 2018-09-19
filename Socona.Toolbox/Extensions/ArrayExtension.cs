﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Extenstions
{

    public static class ArrayExtensions
    {
        public static T[] Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Count(); i++)
            {
                originalArray[i] = with;
            }
            return originalArray;
        }
        public static T[] Fill<T>(this T[] array, int start, int count, T value)
        {


            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (start + count >= array.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            for (var i = start; i < start + count; i++)
            {
                array[i] = value;
            }
            return array;
        }

       
    }

}
