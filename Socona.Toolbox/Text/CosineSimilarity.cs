
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Text
{
   public class CosineSimilarity
    {

        public double GetSimilarity(string fromString, string toString)
        {
            var union = fromString.Union(toString).ToArray();

            int[] fromVector = new int[union.Count()];
            int[] toVector = new int[union.Count()];

            for(int i =0;i<union.Count();i++)
            {
                if (fromString.Contains(union[i]))
                {
                    fromVector[i] = 1;
                }
                if(toString.Contains(union[i]))
                {
                    toVector[i] = 1;
                }
            }

            int sumproduct = 0, sumfrom = 0, sumto = 0;
            for(int i =0;i<union.Count();i++)
            {
                sumproduct += fromVector[i] * toVector[i];
                sumfrom += fromVector[i] * fromVector[i];
                sumto += toVector[i] * toVector[i];
            }
            return (double)sumproduct / (Math.Sqrt(sumfrom) * Math.Sqrt(sumto));
        }
    }
}