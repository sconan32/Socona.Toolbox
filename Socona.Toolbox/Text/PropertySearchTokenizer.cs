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
            while (_idx < _input.Length && (
                _input[_idx] == ' '
                || _input[_idx] == '　'
                || _input[_idx] == '\t'
                || _input[_idx] == '\n'))
            { _idx++; }
            if (_idx >= _input.Length)
            {
                _errors.Add(new TokenizeError() { Start = _idx, Length = 0, Type = TokenizeErrorType.IndexOutOfRange });
                return null;
            }
            TokenItem token = ReadOperator() ?? ReadLiteral();
            return token;
        }
    }
}
