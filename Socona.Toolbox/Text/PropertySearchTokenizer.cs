using Socona.ToolBox.Compiling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Text
{
    public class PropertySearchTokenizer : Tokenizer
    {
        public PropertySearchTokenizer(string input) : base(input)
        {
        }

        public override TokenItem NextToken()
        {
            while (Index < Input.Length && (
                Input[Index] == ' '
                || Input[Index] == '　'
                || Input[Index] == '\t'
                || Input[Index] == '\n'))
            { Index++; }
            if (Index >= Input.Length)
            {
                Errors.Add(new TokenizeError() { Start = Index, Length = 0, Type = TokenizeErrorType.IndexOutOfRange });
                return null;
            }
            TokenItem token = ReadOperator() ?? ReadLiteral();
            return token;
        }
    }
}
