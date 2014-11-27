using System.Collections.Generic;

namespace TextPredict
{
    public class Trie<TValue> : TrieNode<TValue>
    {
        public IEnumerable<TValue> Retrieve(string query)
        {
            return Retrieve(query, 0);
        }

        public void Add(string key, TValue value)
        {
            Add(key, 0, value);
        }
    }
}
