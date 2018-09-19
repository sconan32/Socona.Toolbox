using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Text
{
    public class LevenshteinDistance
    {
        public int ReplaceWeight { get; set; }

        public int AddWeight { get; set; }

        public int DeleteWeight { get; set; }
        public int GetDistance(string fromString, string toString)
        {
            if (fromString == null) fromString = "";
            if (toString == null) toString = "";
            int[,] mem = new int[fromString.Length + 1, toString.Length + 1];
            mem[0, 0] = 0;
            for (int i = 0; i < fromString.Length; i++)
            {
                mem[i + 1, 0] = i + 1;
            }
            for (int j = 0; j < toString.Length; j++)
            {
                mem[0, j + 1] = j + 1;
            }
            for (int i = 0; i < fromString.Length; i++)
            {
                for (int j = 0; j < toString.Length; j++)
                {
                    if (fromString[i] == toString[j])
                    {
                        mem[i + 1, j + 1] = mem[i, j];
                    }
                    else
                    {
                        mem[i + 1, j + 1] = Math.Max(
                            mem[i, j] + ReplaceWeight,
                            Math.Max(mem[i, j + 1] + DeleteWeight, mem[i + 1, j] + AddWeight)
                            );
                    }
                }
            }
            return mem[fromString.Length, toString.Length];
        }

        public double GetSimilarity(string fromString, string toString)
        {
            int total = Math.Max(ReplaceWeight, Math.Max(AddWeight, DeleteWeight));
            return 1.0 - (GetDistance(fromString, toString) / ((double)total * Math.Max(fromString.Length, toString.Length)*2));
        }
    }
}
