using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public class AbstractSyntaxTree
    {
        private AstNodeBase _rootNode;

        private Tokenizer _tokenizer;

        private ParserContext _context;

        private AstNodeBase _errorNode;
        public AbstractSyntaxTree(ParserContext context)
        {
            _context = context;
            _errorNode = new BoolConstantAstNode(true);
            _tokenizer = context.Tokenizer;
            _rootNode = GetOrExpr();
        }

        public Expression Deduce()
        {
            return _rootNode.DeductInternal(_context);
        }

        /// <summary>
        /// OrExpr ::= OrExpr OR AndExpr 
        ///         | AndExpr
        /// We Get OrExpr ::= AndExpr OrExpr1
        ///        OrExpr1 ::= OR AndExpr OrExpr1 
        ///                 | NULL
        /// </summary>
        /// <returns></returns>
        private AstNodeBase GetOrExpr()
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var node = GetAndExpr();
            var nodebase = GetOrExpr1(node);
            return nodebase ?? node;
        }

        private AstNodeBase GetOrExpr1(AstNodeBase leftChild)
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var token = _tokenizer.NextToken();
            if (token?.Operator?.Type == OperatorType.Or)
            {
                var rnode = GetAndExpr();
                AstNodeBase node = new OrElseAstNode() { Token = token, LeftChild = leftChild, RightChild = rnode };

                var root = GetOrExpr1(node);
                return root ?? node;
            }
            _tokenizer.Failback(token);
            return null;
        }
        /// <summary>
        /// AndExpr ::= AndExpr AND UnaryExpr 
        ///         | UnaryExpr
        /// We Get AndExpr ::= UnaryExpr AndExpr1
        ///        AndExpr1 ::= AND UnaryExpr AndExpr1 
        ///                 | NULL
        /// </summary>
        /// <returns></returns>
        private AstNodeBase GetAndExpr()
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var node = GetUnaryExpr();
            var nodebase = GetAndExpr1(node);
            return nodebase ?? node;
        }
        private AstNodeBase GetAndExpr1(AstNodeBase leftChild)
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var token = _tokenizer.NextToken();
            if (token?.Operator?.Type == OperatorType.And)
            {
                var rnode = GetUnaryExpr();
                AstNodeBase node = new AndAlsoAstNode() { Token = token, LeftChild = leftChild, RightChild = rnode };

                var root = GetOrExpr1(node);
                return root ?? node;
            }
            _tokenizer.Failback(token);
            return null;
        }
        /// <summary>
        ///  UnaryExpr ::= NOT UnaryExpr | TRUE | FALSE | CompExpr
        /// </summary>
        /// <returns></returns>
        private AstNodeBase GetUnaryExpr()
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var token = _tokenizer.NextToken();
            if (token?.Operator?.Type == OperatorType.Not)
            {
                var rnode = GetUnaryExpr();
                AstNodeBase node = new NotAstNode() { Token = token, LeftChild = null, RightChild = rnode };
                return node;
            }
            else if (token?.KeyWord == KeyWords.True || token?.KeyWord == KeyWords.False)
            {
                AstNodeBase node = new BoolConstantAstNode() { Token = token };
                return node;
            }
            else
            {
                _tokenizer.Failback(token);
                var node = GetCompExpr();
                return node;
            }
        }
        /// <summary>
        /// CompExpr ::= PropExpr COMP_OP ValExpr 
        ///              | PropExpr
        ///             
        /// </summary>
        /// <returns></returns>
        private AstNodeBase GetCompExpr()
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var lnode = GetPropExpr();
            var token = _tokenizer.NextToken();
            if (Operator.CompareOperators.Contains(token?.Operator?.Type ?? 0))
            {
                var rnode = GetValExpr();
                switch (token?.Operator?.Type)
                {
                    case OperatorType.Eq:
                        return new EqualAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                    case OperatorType.Ne:
                        return new NotEqualAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                    case OperatorType.Gt:
                        return new GreaterThanAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                    case OperatorType.Ge:
                        return new GreaterThanOrEqualAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                    case OperatorType.Lt:
                        return new LessThanAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                    case OperatorType.Le:
                        return new LessThanOrEqualAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                    case OperatorType.FuzzyEq:
                        return new FuzzyEqAstNode() { Token = token, LeftChild = lnode, RightChild = rnode };
                }
            }
            _tokenizer.Failback(token);
            return lnode;
        }
        /// <summary>
        /// PropExpr := PropExpr : STRING
        ///              | : STRING
        /// so We Get 
        /// PropExpr ::= : STRING PropExpr1
        /// PropExpr1 ::= : STRING PropExpr1
        ///             | NULL
        /// 
        /// </summary>
        /// <returns></returns>
        private AstNodeBase GetPropExpr()
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var token = _tokenizer.NextToken();
            if (token?.Operator?.Type == OperatorType.Reference)
            {
                var strtoken = _tokenizer.NextToken();
                AstNodeBase rchild = new StringConstantAstNode { Token = strtoken, DeductedType = typeof(string) };
                AstNodeBase node = new PropertyAstNode() { Token = token, RightChild = rchild, };


                var nodebase = GetPropExpr1(node);
                return nodebase ?? node;
            }
            _tokenizer.Failback(token);
            return _errorNode;
        }

        private AstNodeBase GetPropExpr1(AstNodeBase leftChild)
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var token = _tokenizer.NextToken();
            if (token?.Operator?.Type == OperatorType.Reference)
            {
                var strtoken = _tokenizer.NextToken();
                AstNodeBase rchild = new StringConstantAstNode { Token = strtoken, DeductedType = typeof(string) };
                AstNodeBase node = new PropertyAstNode() { Token = token, RightChild = rchild, };


                var nodebase = GetPropExpr1(node);
                return nodebase ?? node;
            }
            _tokenizer.Failback(token);
            return null;
        }
        /// <summary>
        ///  ValExpr ::= DATE
        ///             | NUMBER
        ///             | BOOL
        ///             | PropExpr
        ///              | STRING            
        ///             
        /// </summary>
        /// <returns></returns>
        private AstNodeBase GetValExpr()
        {
            if (!_tokenizer.HasNext)
            {
                return null;
            }
            var node = GetPropExpr();

            if (node != null)
            {
                return node;
            }

            var token = _tokenizer.NextToken();
            if (token.Type == TokenType.DateLiteral)
            {
                AstNodeBase tnode = new DateConstantAstNode() { Token = token };
                return tnode;
            }
            if (token.Type == TokenType.NumberLiteral)
            {
                AstNodeBase tnode = new NumberConstantAstNode() { Token = token };
                return tnode;
            }
            if (token.Type == TokenType.StringLiteral)
            {
                AstNodeBase tnode = new StringConstantAstNode() { Token = token };

                return tnode;
            }
            _tokenizer.Failback(token);
            return null;
        }
        ///// <summary>
        /////   AlgoExpr ::= AlgoExpr +- MuldivExpr
        /////                | PropExpr +- MuldivExpr
        /////                | PropExpr +- DATETIME
        /////                | DATETIME
        /////                | MuldivExpr
        /////                
        ///// </summary>
        ///// <returns></returns>
        //private AstNodeBase GetAlgoExpr()
        //{

        //}

    }


}
