using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Socona.ToolBox.Extenstions
{
    public static class BitSetExt
    {
        /// <summary>
        /// Calculate the Set bit count;
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int Cardinality(this BitArray arr)
        {
            int ret = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i])
                {
                    ret++;
                }
            }
            return ret;
        }
        public static int NextClearBit(this BitArray arr, int index)
        {
            int ret = -1;
            for (int i = index; i < arr.Count; i++)
            {
                if (arr[i] == false)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }
        public static int NextTrueIndex(this BitArray arr, int index)
        {
            int ret = -1;
            for (int i = index; i < arr.Count; i++)
            {
                if (arr[i] == true)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }
        public static void Flip(this BitArray arr, int from, int to)
        {
            for (int i = from; i < to && i < arr.Count; i++)
            {
                arr[i] = !arr[i];
            }
        }
    }
}
