using Socona.ToolBox.Compiling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Socona.ToolBox.Compiling
{
    public abstract class Tokenizer
    {
        protected string _input;

        protected int _idx;

        protected List<TokenizeError> _errors = new List<TokenizeError>();

        public bool HasError
        {
            get { return _errors.Count > 0; }
        }

        public bool HasNext
        {
            get; private set;
        }
        public Tokenizer(string input)
        {
            _input = input + "$";
            _idx = 0;
            HasNext = true;
        }
        //分析运算符
        public virtual TokenItem ReadOperator()
        {
            if (_input[_idx] == ':')
            {
                _idx++;
                return new TokenItem(OperatorType.Reference);
            }
            else if (_input[_idx] == '(')
            {
                _idx++;
                return new TokenItem(OperatorType.LParen);
            }
            else if (_input[_idx] == ')')
            {
                _idx++;
                return new TokenItem(OperatorType.RParen);
            }
            else if (_input[_idx] == '+')
            {
                _idx++;
                return new TokenItem(OperatorType.Plus);
            }
            else if (_input[_idx] == '-')
            {
                _idx++;
                return new TokenItem(OperatorType.Minus);
            }
            else if (_input[_idx] == '*')
            {
                if (_input[_idx + 1] == '*')
                {
                    _idx += 2;
                    return new TokenItem(OperatorType.Power);
                }
                _idx++;
                return new TokenItem(OperatorType.Multiply);
            }
            else if (_input[_idx] == '/')
            {
                _idx++;
                return new TokenItem(OperatorType.Divide);
            }
            else if (_input[_idx] == '%')
            {
                _idx++;
                return new TokenItem(OperatorType.Minus);
            }
            else if (_input[_idx] == '*')
            {
                _idx++;
                return new TokenItem(OperatorType.Multiply);
            }
            else if (_input[_idx] == '<')
            {
                if (_input[_idx + 1] == '=')
                {
                    _idx += 2;
                    return new TokenItem(OperatorType.Le);
                }
                _idx++;
                return new TokenItem(OperatorType.Lt);
            }
            else if (_input[_idx] == '>')
            {
                if (_input[_idx + 1] == '=')
                {
                    _idx += 2;
                    return new TokenItem(OperatorType.Ge);
                }
                _idx++;
                return new TokenItem(OperatorType.Gt);
            }
            else if (_input[_idx] == '!')
            {
                if (_input[_idx + 1] == '=')
                {
                    _idx += 2;
                    return new TokenItem(OperatorType.Ne);
                }
                _idx++;
                return new TokenItem(OperatorType.Not);
            }
            else if (_input[_idx] == '=')
            {
                _idx++;
                return new TokenItem(OperatorType.Eq);
            }
            else if (_input[_idx] == '~')
            {
                _idx++;
                return new TokenItem(OperatorType.FuzzyEq);
            }
            else if (_input[_idx] == '$')
            {
                HasNext = false;
                return new TokenItem(OperatorType.END_STACK);
            }

            else
            {
                return null;
            }
        }

        public virtual TokenItem ReadString()
        {
            string result = "";
            Regex regStr = new Regex(@"[\p{IsCJKUnifiedIdeographs}A-Za-z][\p{IsCJKUnifiedIdeographs}A-Za-z0-9]*", RegexOptions.Compiled);
            var match = regStr.Match(_input, _idx);
            if (match.Success)
            {
                result = match.Value;
                _idx += result.Length;
            }
            return new TokenItem { Type = TokenType.StringLiteral, TokenString = result, };
        }

        public virtual TokenItem ReadNumber()
        {
            Regex regStr = new Regex(@"[+-]?\d+[.]?\d*", RegexOptions.Compiled);
            var match = regStr.Match(_input, _idx);
            if (match.Success)
            {
                string result = match.Value;
                _idx += result.Length;
                return new TokenItem { Type = TokenType.StringLiteral, TokenString = result, };
            }
            return null;
        }

        public virtual TokenItem ReadDate()
        {

            Regex[] patterns = {
              new Regex(  "^(?<year>\\d{2,4})/(?<month>\\d{1,2})/(?<day>\\d{1,2})$", RegexOptions.Compiled),// yyyy/MM/dd  
                new Regex(   "^(?<year>\\d{2,4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})$",RegexOptions.Compiled),    // yyyy-MM-dd   
                new Regex(   "^(?<year>\\d{2,4})[.](?<month>\\d{1,2})[.](?<day>\\d{1,2})$", RegexOptions.Compiled),    // yyyy.MM.dd   
                new Regex(   "^((?<year>\\d{2,4})年)?(?<month>\\d{1,2})月((?<day>\\d{1,2})日)?$", RegexOptions.Compiled),// yyyy年MM月dd日  
                // "^((?<year>\\d{2,4})年)?(正|一|二|三|四|五|六|七|八|九|十|十一|十二)月((一|二|三|四|五|六|七|八|九|十){1,3}日)?$"
                //"^(零|〇|一|二|三|四|五|六|七|八|九|十){2,4}年((正|一|二|三|四|五|六|七|八|九|十|十一|十二)月((一|二|三|四|五|六|七|八|九|十){1,3}(日)?)?)?$"
            };

            for (int i = 0; i < patterns.Length; i++)
            {
                var regex = patterns[i];
                var match = regex.Match(_input, _idx);
                if (match.Success)
                {
                    _idx += match.Value.Length;
                    var result = match.Groups["year"] + "/" + match.Groups["month"] + "/" + match.Groups["day"];
                    return new TokenItem { Type = TokenType.DateLiteral, TokenString = result, };
                }

            }
            return null;

        }
        public abstract TokenItem NextToken();


        protected virtual TokenItem ReadLiteral()
        {
            var token = ReadDate() ?? ReadNumber() ?? ReadString();
            return token;
        }



        public virtual void Failback(TokenItem token)
        {
            _idx -= token.TokenString.Length;
        }
    }
    public struct TokenizeError
    {
        public int Start { get; set; }
        public int Length { get; set; }

        public TokenizeErrorType Type { get; set; }
    }

    public enum TokenizeErrorType
    {
        Unkonwn = 0,
        InvalidOperator = 1,



        IndexOutOfRange = 0x10000,
    }
}
