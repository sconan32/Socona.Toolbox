using Socona.ToolBox.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Compiling
{

    public class PropertyItem
    {
        public string Target { get; set; }

        public Type TargetType { get; set; }


        public string Source { get; set; }




        public bool IsMatch(string token)
        {
            return Source == token;
        }

        public double FuzzyMatch(string token)
        {
            if (IsMatch(token)) return 1;
            var dist = (new LevenshteinDistance()).GetDistance(Source, token);
            int maxLen = Math.Max(Source.Length, token.Length);
            return (maxLen - dist) / (double)maxLen;


        }

    }
}
