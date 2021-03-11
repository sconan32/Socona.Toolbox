using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Collections
{
    public class Trie
    {
        private TrieNode _root;

        private readonly string[] _keywords;

        public Trie(IEnumerable<string> keywords)
        {
            _keywords = keywords.ToArray();
            Initialize();
        }

        private void Initialize()
        {
            _root = new TrieNode(' ', null);

            // 添加模式
            foreach (var keyword in _keywords)
            {
                var node = _root;
                foreach (var ch in keyword)
                {
                    TrieNode temp = null;
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
                        temp = new TrieNode(ch, node);
                        node.AddTransition(temp);
                    }
                    node = temp;
                }
                node.AddResult(keyword);
            }

            // 第一层失败指向根节点
            var nodes = new List<TrieNode>();
            foreach (var node in _root.Transitions)
            {
                // 失败指向root
                node.Failure = _root;
                foreach (var trans in node.Transitions) nodes.Add(trans);
            }

            // 其它节点 BFS
            while (nodes.Count != 0)
            {
                var newNodes = new List<TrieNode>();
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
    }

    /// <summary>
    ///     构造节点
    /// </summary>
    internal class TrieNode
    {
        private readonly Dictionary<char, TrieNode> transDict;

        public TrieNode(char c, TrieNode parent)
        {
            Char = c;
            Parent = parent;
            Transitions = new List<TrieNode>();
            Results = new List<string>();

            transDict = new Dictionary<char, TrieNode>();
        }

        public char Char { get; }

        public TrieNode Parent { get; }

        public TrieNode Failure { get; set; }

        public List<TrieNode> Transitions { get; private set; }

        public List<string> Results { get; }

        public void AddResult(string result)
        {
            if (!Results.Contains(result)) Results.Add(result);
        }

        public void AddTransition(TrieNode node)
        {
            transDict.Add(node.Char, node);
            Transitions = transDict.Values.ToList();            
        }



        public TrieNode GetTransition(char c)
        {
            TrieNode node;
            if (transDict.TryGetValue(c, out node)) return node;
            return null;
        }

        public bool ContainsTransition(char c)
        {
            return GetTransition(c) != null;
        }
    }
}
