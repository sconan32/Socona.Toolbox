using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Compiling
{
    public abstract class AstNodeBase
    {
        public Type PreferredType { get; set; }

        public Type DeductedType { get; set; }


        public TokenItem Token { get; set; }
        public AstNodeBase Parent { get; set; }



        private AstNodeBase _leftChild;
        public AstNodeBase LeftChild
        {
            get { return _leftChild; }
            set
            {
                _leftChild = value;
                if (_leftChild != null)
                {
                    _leftChild.Parent = this;
                }
            }
        }
        private AstNodeBase _rightChild;
        public AstNodeBase RightChild
        {
            get { return _rightChild; }
            set
            {
                _rightChild = value;
                if (_rightChild != null)
                {
                    _rightChild.Parent = this;
                }
            }
        }

        //分析期可求值的将在分析期求值
        public object Value { get; set; }

        //分析期不可求值的将转化为表达式在执行时求值
        public Expression Expression { get; set; }

        public abstract Expression DeductInternal(object context);

    }



    public class BoolConstantAstNode : AstNodeBase
    {
        public BoolConstantAstNode() { }


        public BoolConstantAstNode(bool val)
        {
            Value = val;
            if (val)
            {
                Token = new TokenItem { KeyWord = KeyWords.True, Type = TokenType.Keyword, TokenString = "true", };

            }
            else
            {
                Token = new TokenItem { KeyWord = KeyWords.False, Type = TokenType.Keyword, TokenString = "false", };
            }
        }

        public override Expression DeductInternal(object context)
        {
            if (Token.Type == TokenType.Keyword)
            {
                if (Token.KeyWord == KeyWords.True)
                {
                    DeductedType = typeof(bool?);
                    Value = true;
                    return Expression.Constant(true);
                }
                else if (Token.KeyWord == KeyWords.False)
                {

                    DeductedType = typeof(bool?);
                    Value = false;
                    return Expression.Constant(false);
                }
            }
            Value = null;
            return Expression.Constant(null);
        }

    }
    public class StringConstantAstNode : AstNodeBase
    {
        public override Expression DeductInternal(object context)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberConstantAstNode:AstNodeBase
    {
        public override Expression DeductInternal(object context)
        {
            throw new NotImplementedException();
        }
    }

    public class DateConstantAstNode:AstNodeBase
    {
        public override Expression DeductInternal(object context)
        {
            throw new NotImplementedException();
        }
    }
    public class BinaryInternalAstNode : AstNodeBase
    {

        protected Func<Expression, Expression, BinaryExpression> Op;
        public BinaryInternalAstNode(Func<Expression, Expression, BinaryExpression> op)
        {
            Op = op;
        }

        public override Expression DeductInternal(object context)
        {
            DeductedType = typeof(bool);
            var lhs = LeftChild.DeductInternal(context);
            var rhs = RightChild.DeductInternal(context);
            return Op(lhs, rhs);
        }
    }

    public class AndAlsoAstNode : BinaryInternalAstNode
    {
        public AndAlsoAstNode() : base(Expression.AndAlso) { }
    }
    public class OrElseAstNode : BinaryInternalAstNode
    {
        public OrElseAstNode() : base(Expression.OrElse) { }
    }
    public class NotAstNode : AstNodeBase
    {
        public override Expression DeductInternal(object context)
        {
            DeductedType = typeof(bool);
            var rhs = RightChild.DeductInternal(context);
            return Expression.Not(rhs);
        }
    }

    public class EqualAstNode : BinaryInternalAstNode
    {
        public EqualAstNode() : base(Expression.Equal) { }
    }

    public class NotEqualAstNode : BinaryInternalAstNode
    {
        public NotEqualAstNode() : base(Expression.NotEqual) { }
    }
    public class GreaterThanAstNode : BinaryInternalAstNode
    {
        public GreaterThanAstNode() : base(Expression.GreaterThan) { }
    }
    public class GreaterThanOrEqualAstNode : BinaryInternalAstNode
    {
        public GreaterThanOrEqualAstNode() : base(Expression.GreaterThanOrEqual) { }
    }
    public class LessThanAstNode : BinaryInternalAstNode
    {
        public LessThanAstNode() : base(Expression.LessThan) { }
    }
    public class LessThanOrEqualAstNode : BinaryInternalAstNode
    {
        public LessThanOrEqualAstNode() : base(Expression.LessThanOrEqual) { }
    }

    public class FuzzyEqAstNode : AstNodeBase
    {
        public override Expression DeductInternal(object context)
        {
            if (LeftChild.DeductedType == typeof(string))
            {
                var lhs = LeftChild.DeductInternal(context);
                var rhs = RightChild.DeductInternal(context);
                MethodInfo tostring = RightChild.DeductedType.GetMethod("ToString");
                MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                return Expression.Call(
                    lhs,
                    contains,
                    Expression.Call(rhs, tostring)
                    );
            }
            Value = null;
            return Expression.Constant(null);
        }
    }

   
    public class PropertyAstNode : AstNodeBase
    {
        public override Expression DeductInternal(object context)
        {
            var map = (context as ParserContext).Properties;
            var lhs = LeftChild.DeductInternal(context);
            var rhs = RightChild.DeductInternal(context);
            PropertyMap prs = null;
            if (LeftChild.Value == null)
            {
                prs = map["Root"];
                var prop = prs.BestMatch(RightChild.Value.ToString());
                if (prop != null)
                {
                    DeductedType = prop.TargetType;
                    return Expression.Parameter(prop.TargetType,"t");
                }
            }
            else
            {
                prs = map[LeftChild.DeductedType.Name];
            }
            //存在对应的属性列表
            if (prs != null)
            {
                var prop = prs.BestMatch(RightChild.Value.ToString());
                if (prop != null)
                {
                    DeductedType = prop.TargetType;
                    return Expression.Property(lhs, RightChild.Value.ToString());
                }                
            }
            //只有当前的域没有高级限定符时，才进行模糊查找 
            if (LeftChild == null)
            {
                // 如果不存在，应从最近的有左子树的父结点开始，找他的最左子树的是PropertyAstNode结点，
                // 判断其DeductedType只否有属性列表
                var pNode = this.Parent;
                AstNodeBase last = this;
                while (pNode != null)
                {
                    //last是右子树说明pNode有已经推断的左孩子可能是Property属性
                    // 如果 是左子树，则其右子树尚未推断，不符合我们的语法要求,不去访问
                    if (last == pNode.RightChild)
                    {
                        var lch = pNode.LeftChild;

                        //一直判断其左孩子如果不为空（非叶子结点），并且其类型是PropertyAstNode
                        //那么尝试获取其属性列表，一旦不为空，匹配是否有合适的属性，若有返回

                        while (lch.LeftChild != null)
                        {
                            if (lch is PropertyAstNode)
                            {
                                prs = map[lch.DeductedType.Name];
                                if (prs != null)
                                {
                                    var prop = prs.BestMatch(RightChild.Value.ToString());
                                    if (prop != null)
                                    {
                                        DeductedType = prop.TargetType;
                                        return Expression.Property(lhs, RightChild.Value.ToString());
                                    }
                                    //未找到 可能是更高层级的域                                 
                                }
                            }
                            lch = lch.LeftChild;
                        }
                    }

                    last = pNode;
                    pNode = pNode.Parent;
                }
            }
            
            //遍历所有的结点均未找到合适的属性返回空
            Value = null;
            return Expression.Constant(null);

        }
    }

}
