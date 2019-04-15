using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public class Interpreter
    {
        private AbstractSyntaxTree _astree;
        private ParserContext _context;

        public Interpreter(ParserContext context)
        {
            _context = context;
        }
        public Type FirstDeductedType { get; set; }
        public Expression GetExpression(string input)
        {
            _astree = new AbstractSyntaxTree(input);
            var expr= _astree.Deduce(_context);

            return expr;
        }

    }
}
