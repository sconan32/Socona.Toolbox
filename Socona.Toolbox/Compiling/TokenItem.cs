using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public class TokenItem
    {
        public TokenItem()
        {

        }
        public TokenItem(OperatorType opType)
        {
            Type = TokenType.Operator;
            Operator = Operator.Operators[opType];
            TokenString = Operator.OperatorString;
        }

        public string TokenString { get; set; }

        public Operator Operator { get; set; }
        public object Operand { get; set; }

        public TokenType Type { get; set; }

        public KeyWords KeyWord{ get; set; }

    }


    public enum TokenType
    {

        Keyword = 0x0001,
        Operator = 0x0002,
        BoolLiteral=0x0004,
        NumberLiteral=0x0008,
        StringLiteral=0x0010,
        DateLiteral=0x0020,
        DateTimeLiteral=0x0040,


    }

}
