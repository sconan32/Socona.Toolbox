using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Socona.ToolBox.Extenstions
{
    public static class ListExtension
    {

        public static void Shuffle<T>(this List<T> list, Random rand)
        {
            for (int i = list.Count - 1; i-- > 1;)
            {
                var tmpidx = rand.Next(i);
                T temp = list[i];
                list[i] = list[tmpidx];
                list[tmpidx] = temp;
            }

        }
    }
}
