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
                    Node temp = node.GetTransition(ch);
                    if (temp == null)
                    {
                        temp = new Node(ch, node);
                        node.AddTransition(temp);
                    }
                    node = temp;
                }
                node.Result = keyword;
            }

            //BFS 第一层失败指向根节点
            var nodes = new Queue<Node>();
            foreach (var node in _root.Transitions)
            {
                // 失败指向root
                node.Failure = _root;
                foreach (var trans in node.Transitions)
                {
                    nodes.Enqueue(trans);
                }
            }

            // 其它节点 BFS
            while (nodes.Count != 0)
            {
                var nd = nodes.Dequeue();
                foreach (var child in nd.Transitions)
                {
                    nodes.Enqueue(child);
                    Node failure = nd.Failure;//从当前孩子的公共子序列找起
                    while (true)
                    {
                        if (failure == null) //只有根节点的Failure为null
                        {
                            child.Failure = _root;
                            break;
                        }

                        if (failure.ContainsTransition(child.Char)) //含有公共前缀
                        {
                            child.Failure = failure.GetTransition(child.Char);
                            break;
                        }
                        else //继续向上寻找
                        {
                            failure = failure.Failure;
                        }
                    }
                }
            }
            // 根节点的失败指向自己
            // _root.Failure = _root;
        }

        public bool IsMatchAny(string text)
        {
            var current = _root;
            for (var index = 0; index < text.Length; ++index)
            {
                char ch = text[index];
                var trans = current.GetTransition(ch);
                if (trans != null)
                {
                    current = trans;

                    if (current.HasKeyword)
                    {
                        return true;
                    }
                    if (current.Failure != null && current.Failure.HasKeyword)
                    {
                        return true;
                    }
                    index++;
                }
                else
                {
                    current = current.Failure;
                    if (current == null)
                    {
                        current = _root;
                        index++;
                    }
                }
            }
            return false;
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
                char ch = text[index];
                var trans = current.GetTransition(ch);
                if (trans != null)
                {
                    current = trans;

                    if (current.HasKeyword)
                    {
                        list.Add(new KeywordSearchResult(index - current.Result.Length + 1, current.Result));
                    }
                    if (current.Failure != null & current.Failure.HasKeyword)
                    {
                        list.Add(new KeywordSearchResult(index - current.Failure.Result.Length + 1, current.Failure.Result));
                    }
                    index++;
                }
                else
                {
                    current = current.Failure;
                    if (current.Failure == null)
                    {
                        current = _root;
                        index++;
                    }
                }
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

            public bool HasKeyword => !string.IsNullOrEmpty(Result);


            public string Result { get; set; }

            public void AddTransition(Node node)
            {
                transDict.Add(node.Char, node);
            }

            public Node GetTransition(char c)
            {
                return transDict.TryGetValue(c, out Node node) ? node : null;
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
