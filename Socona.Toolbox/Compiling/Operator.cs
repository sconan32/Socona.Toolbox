using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public class Operator
    {
        private Operator() { }

        public int OperandCount;        // 操作数个数
        public int IncomingPrededence;            // 优先级  
        public int InStackPrecedence;            // 优先级  
        public Associativity Associativity;  // 结合性  
        public OperatorType Type; // 操作符  
        public string OperatorString;

        public static readonly List<OperatorType> CompareOperators = new List<OperatorType>()
        {
             OperatorType.Eq, OperatorType.Ne, OperatorType.FuzzyEq, OperatorType.Ge,
              OperatorType.Gt, OperatorType.Le, OperatorType.Lt,
        };
        public static readonly Dictionary<OperatorType, Operator> Operators =
        new Dictionary<OperatorType, Operator> {
             {   // ::
                OperatorType.Reference,
                new Operator{ OperandCount = 2, IncomingPrededence = 16, InStackPrecedence = 16, Associativity = Associativity.Left2Right, Type = OperatorType.Reference ,OperatorString=":",}
            },
            //算数运算             
            {   // ( 左括号
                OperatorType.LParen,
                new Operator{ OperandCount = 2, IncomingPrededence = 17, InStackPrecedence = 1, Associativity = Associativity.Left2Right, Type = OperatorType.LParen,OperatorString="(", }
            },
            {   // ) 右括号
                OperatorType.RParen,
                new Operator{ OperandCount=2,  IncomingPrededence=17, InStackPrecedence=17, Associativity= Associativity.Left2Right, Type= OperatorType.RParen,OperatorString=")",}
            },
            {   // + 加
                OperatorType.Plus,
                new Operator{ OperandCount=2,  IncomingPrededence=12, InStackPrecedence=12, Associativity= Associativity.Left2Right, Type= OperatorType.Plus,OperatorString="+",}
            },
            {   // - 减
                OperatorType.Minus,
                new Operator{ OperandCount=2,  IncomingPrededence=12, InStackPrecedence=12, Associativity= Associativity.Left2Right, Type= OperatorType.Minus,OperatorString="-",}
            },
            {   // * 乘
                OperatorType.Multiply,
                new Operator{ OperandCount=2,  IncomingPrededence=13, InStackPrecedence=13, Associativity= Associativity.Left2Right, Type= OperatorType.Multiply,OperatorString="*",}
            },
            {   // / 除
                OperatorType.Divide,
                new Operator{ OperandCount=2,  IncomingPrededence=13, InStackPrecedence=13, Associativity= Associativity.Left2Right, Type= OperatorType.Divide,OperatorString="/",}
            },
            {  // % 
                OperatorType.Mod,
                new Operator{ OperandCount=2,  IncomingPrededence=13, InStackPrecedence=13, Associativity= Associativity.Left2Right, Type= OperatorType.Mod,OperatorString="%",}
            },
            {   // ** 
                OperatorType.Power,
                new Operator{ OperandCount=2,  IncomingPrededence=14, InStackPrecedence=14, Associativity= Associativity.Left2Right, Type= OperatorType.Power,OperatorString="**",}
            },
            {   // + 
                OperatorType.Positive,
                new Operator{ OperandCount=1,  IncomingPrededence=16, InStackPrecedence=15, Associativity= Associativity.Right2Left, Type= OperatorType.Positive,OperatorString="+",}
            },
            {   // - 
                OperatorType.Negative,
                new Operator{ OperandCount=1,  IncomingPrededence=16, InStackPrecedence=15, Associativity= Associativity.Right2Left, Type= OperatorType.Negative,OperatorString="-",}
            },
            {   // !! 
                OperatorType.Factorial,
                new Operator{ OperandCount=1,  IncomingPrededence=16, InStackPrecedence=15, Associativity= Associativity.Left2Right, Type= OperatorType.Factorial,OperatorString="!!",}
            },
            //关系运算
            {   // < 
                OperatorType.Lt,
                new Operator{ OperandCount=2,  IncomingPrededence=10, InStackPrecedence=10, Associativity= Associativity.Left2Right, Type= OperatorType.Lt,OperatorString="<",}
            },
            {   // > 
                OperatorType.Gt,
                new Operator{ OperandCount=2,  IncomingPrededence=10, InStackPrecedence=10, Associativity= Associativity.Left2Right, Type= OperatorType.Gt,OperatorString=">",}
            },
            {   // == 
                OperatorType.Eq,
                new Operator{ OperandCount=2,  IncomingPrededence=9, InStackPrecedence=9, Associativity= Associativity.Left2Right, Type= OperatorType.Eq,OperatorString="=",}
            },
            {   // ?=   
                OperatorType.FuzzyEq,
                new Operator{ OperandCount=2,  IncomingPrededence=9, InStackPrecedence=9, Associativity= Associativity.Left2Right, Type= OperatorType.FuzzyEq,OperatorString="~=",}
            },
            {   // != 
                OperatorType.Ne,
                new Operator{ OperandCount=2,  IncomingPrededence=9, InStackPrecedence=9, Associativity= Associativity.Left2Right, Type= OperatorType.Ne,OperatorString="!=",}
            },
            {   // <= 
                OperatorType.Le,
                new Operator{ OperandCount=2,  IncomingPrededence=10, InStackPrecedence=10, Associativity= Associativity.Left2Right, Type= OperatorType.Le,OperatorString="<=",}
            },
            {   // >= 
                OperatorType.Ge,
                new Operator{ OperandCount=2,  IncomingPrededence=10, InStackPrecedence=10, Associativity= Associativity.Left2Right, Type= OperatorType.Ge,OperatorString=">=",}
            },

            //逻辑运算
            {   // &&
                OperatorType.And,
                new Operator{ OperandCount=2,  IncomingPrededence=5, InStackPrecedence=5, Associativity= Associativity.Left2Right, Type= OperatorType.And,OperatorString="&&",}
            },
            {   //  ||   
                OperatorType.Or,
                new Operator{ OperandCount=2,  IncomingPrededence=4, InStackPrecedence=4, Associativity= Associativity.Left2Right, Type= OperatorType.Or,OperatorString="||",}
            },
            {   // ! 
                OperatorType.Not,
                new Operator{ OperandCount=2,  IncomingPrededence=15, InStackPrecedence=15, Associativity= Associativity.Right2Left, Type= OperatorType.Not,OperatorString="!",}
            },
            //赋值
            //{   // = 
            //    OperatorType.Assignment,
            //    new Operator{ Operand=2,  IncomingPrededence=17, InStackPrecedence=1, Associativity= Associativity.Right2Left, Type= OperatorType.Assignment}
            //},

            {   // ENDSTACK
                OperatorType.END_STACK,
                new Operator{ OperandCount=2,  IncomingPrededence=0, InStackPrecedence=0, Associativity= Associativity.Right2Left, Type= OperatorType.END_STACK,OperatorString="$", }
            },
        };


    }
    public enum Associativity
    {
        Left2Right,
        Right2Left
    }


    public enum OperatorType : uint
    {
        Reference = 1,  //访问对象
        /* 算数运算 */
        LParen,    // 左括号  
        RParen,        // 右括号  
        Plus,          // 加  
        Minus,         // 减  
        Multiply,      // 乘  
        Divide,        // 除  
        Mod,           // 模  
        Power,         // 幂  
        Positive,      // 正号  
        Negative,      // 负号  
        Factorial,     // 阶乘  
                       /* 关系运算 */
        Lt,            // 小于  
        Gt,            // 大于  
        Eq,            // 等于  
        FuzzyEq,            //约 等于  
        Ne,            // 不等于  
        Le,            // 不大于  
        Ge,            // 不小于  
                       /* 逻辑运算 */
        And,           // 且  
        Or,            // 或  
        Not,           // 非  
                       /* 赋值 */
        Assignment,    // 赋值  
        END_STACK = 0xFFFFFFFF,   // 栈底  
    }
}
