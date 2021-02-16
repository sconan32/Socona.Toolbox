using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Text
{
    public class LevenshteinDistance
    {
        public int ReplaceWeight { get; set; } = 1;

        public int AddWeight { get; set; } = 1;

        public int DeleteWeight { get; set; } = 1;
        public int GetDistance(string source, string target)
        {
            if (source == null) source = "";
            if (target == null) target = "";
            int[,] mem = new int[source.Length + 1, target.Length + 1];
            mem[0, 0] = 0;
            for (int i = 0; i <= source.Length; i++)
            {
                mem[i , 0] = i ;
            }
            for (int j = 0; j <= target.Length; j++)
            {
                mem[0, j ] = j;
            }
            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {

                    int cost = (target[j - 1] == source[i - 1]) ? 0 : ReplaceWeight;

                    mem[i, j] = Math.Min(
                        mem[i - 1, j - 1] + cost,
                        Math.Min(mem[i - 1, j] + DeleteWeight, mem[i, j - 1] + AddWeight)
                        );

                }
            }
            return mem[source.Length, target.Length];
        }

        public double GetSimilarity(string fromString, string toString)
        {
            int total = Math.Max(ReplaceWeight, Math.Max(AddWeight, DeleteWeight));
            return 1.0 - (GetDistance(fromString, toString) / ((double)total * Math.Max(fromString.Length, toString.Length) * 2));
        }
    }
}
