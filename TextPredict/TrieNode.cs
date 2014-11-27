using System;
using System.Collections.Generic;
using System.Linq;

namespace TextPredict
{
    public class TrieNode<T> 
    {
        private readonly Dictionary<char, TrieNode<T>> m_Children;
        private readonly Queue<T> m_Values;
        protected int KeyLength { get { return 1; }  }
        protected IEnumerable<TrieNode<T>> Children() { return m_Children.Values; }
        protected IEnumerable<T> Values() { return m_Values; }

        protected TrieNode()
        {
            m_Children = new Dictionary<char, TrieNode<T>>();
            m_Values = new Queue<T>();
        }

        public void Add(string key, int position, T value)
        {
            if (key == null) throw new ArgumentNullException();
            if (EndOfString(position, key))
            {
                AddValue(value);
                return;
            }

            TrieNode<T> child = GetOrCreateChild(key[position]);
            child.Add(key, position + 1, value);
        }

        protected IEnumerable<T> Retrieve(string query, int position)
        {
            return EndOfString(position, query) ? ValuesDeep() : SearchDeep(query, position);
        }

        protected IEnumerable<T> SearchDeep(string query, int position)
        {
            TrieNode<T> nextNode = GetChildOrNull(query, position);
            return nextNode != null ? nextNode.Retrieve(query, position + nextNode.KeyLength) : Enumerable.Empty<T>();
        }
        private IEnumerable<T> ValuesDeep()
        {
            return Subtree().SelectMany(node => node.Values());
        }
        protected IEnumerable<TrieNode<T>> Subtree()
        {
            return Enumerable.Repeat(this, 1).Concat(Children().SelectMany(child => child.Subtree()));
        }
        protected TrieNode<T> GetOrCreateChild(char key)
        {
            TrieNode<T> result;
            if (!m_Children.TryGetValue(key, out result))
            {
                result = new TrieNode<T>();
                m_Children.Add(key, result);
            }
            return result;
        }
        protected TrieNode<T> GetChildOrNull(string query, int position)
        {
            if (query == null) throw new ArgumentNullException();
            TrieNode<T> childNode;
            return m_Children.TryGetValue(query[position], out childNode) ? childNode : null;
        }
        protected void AddValue(T value)  { m_Values.Enqueue(value); }
        private static bool EndOfString(int position, string text) {  return position >= text.Length; }
    }
}
