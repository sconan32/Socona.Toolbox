using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Text
{
    public class PatternCollection
    {
        private Node _root;

        private readonly string[] _keywords;

        public PatternCollection(IEnumerable<string> keywords)
        {

            _keywords = keywords.ToArray();
            Initialize();
        }

        private void Initialize()
        {
            _root = new Node(' ', null);

            // 添加模式
            foreach (var keyword in _keywords)
            {
                var node = _root;
                foreach (var ch in keyword)
                {
                    Node temp = null;
                    foreach (var tnode in node.Transitions)
                    {
                        if (tnode.Char == ch)
                        {
                            temp = tnode;
                            break;
                        }
                    }
                    if (temp == null)
                    {
                        temp = new Node(ch, node);
                        node.AddTransition(temp);
                    }
                    node = temp;
                }
                node.AddResult(keyword);
            }

            // 第一层失败指向根节点
            var nodes = new List<Node>();
            foreach (var node in _root.Transitions)
            {
                // 失败指向root
                node.Failure = _root;
                foreach (var trans in node.Transitions) nodes.Add(trans);
            }

            // 其它节点 BFS
            while (nodes.Count != 0)
            {
                var newNodes = new List<Node>();
                foreach (var nd in nodes)
                {
                    var r = nd.Parent.Failure;
                    var c = nd.Char;

                    while (r != null && !r.ContainsTransition(c)) r = r.Failure;

                    if (r == null)
                    {
                        // 失败指向root
                        nd.Failure = _root;
                    }
                    else
                    {
                        nd.Failure = r.GetTransition(c);
                        foreach (var result in nd.Failure.Results) nd.AddResult(result);
                    }
                    foreach (var child in nd.Transitions) newNodes.Add(child);
                }
                nodes = newNodes;
            }
            // 根节点的失败指向自己
            _root.Failure = _root;
        }

        public bool IsMatchAny(string text)
        {
            return FindAllKeywords(text).Count > 0;
        }

        /// <summary>
        ///     找出所有出现过的关键词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<KeywordSearchResult> FindAllKeywords(string text)
        {
            var list = new List<KeywordSearchResult>();

            var current = _root;
            for (var index = 0; index < text.Length; ++index)
            {
                Node trans;
                do
                {
                    trans = current.GetTransition(text[index]);

                    if (current == _root)
                        break;

                    if (trans == null) current = current.Failure;
                } while (trans == null);

                if (trans != null) current = trans;

                foreach (var s in current.Results) list.Add(new KeywordSearchResult(index - s.Length + 1, s));
            }

            return list;
        }

        /// <summary>
        ///     构造节点
        /// </summary>
        private class Node
        {
            private readonly Dictionary<char, Node> transDict;

            public Node(char c, Node parent)
            {
                Char = c;
                Parent = parent;
                Results = new List<string>();
               
                transDict = new Dictionary<char, Node>();
            }

            public char Char { get; }

            public Node Parent { get; }

            public Node Failure { get; set; }

            public IEnumerable<Node> Transitions
            {
                get
                {
                    foreach (var node in transDict.Values)
                    {
                        yield return node;

                    }
                }
            }

            public List<string> Results { get; }

            public void AddResult(string result)
            {
                if (!Results.Contains(result)) Results.Add(result);
            }

            public void AddTransition(Node node)
            {
                transDict.Add(node.Char, node);            
            }

            public Node GetTransition(char c)
            {
                Node node;
                if (transDict.TryGetValue(c, out node)) return node;

                return null;
            }

            public bool ContainsTransition(char c)
            {
                return GetTransition(c) != null;
            }
        }
    }




    /// <summary>
    ///     表示一个查找结果
    /// </summary>
    public struct KeywordSearchResult
    {
        public static readonly KeywordSearchResult Empty = new KeywordSearchResult(-1, string.Empty);

        public KeywordSearchResult(int index, string keyword)
        {
            Index = index;
            Keyword = keyword;
        }

        /// <summary>
        ///     位置
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///     关键词
        /// </summary>
        public string Keyword { get; }
    }
}
