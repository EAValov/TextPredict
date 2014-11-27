using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TextPredict
{
    class Program
    {
        public struct WordAndFrequency
        {
            public int Frequency;
            public string Word;
        }
        static void Main(string[] args)
        {
            string path = Console.ReadLine().Trim().ToLower();

            Trie<WordAndFrequency> trie = new Trie<WordAndFrequency>();

            List<string> inputs = new List<string>();
            
            ParseFile(path, ref trie, ref inputs);
            ShowGueses(inputs, trie);

        }

        static void ShowGueses(List<string> Guesses, Trie<WordAndFrequency> trie)
        {
            for (int c = 0; c < Guesses.Count; c++)
            {
                var result = trie.Retrieve(Guesses[c]);

                result.OrderByDescending( r => r.Frequency);
                var variants = (result
                    .Take(10)
                    .OrderByDescending(t => t.Frequency)
                    .ThenBy(r => r.Word)
                    .Select(y=>y.Word))
                    .ToArray();

                for (int y = 0; y < variants.Length; y++)
                    Console.WriteLine(variants[y]);
                  
               Console.WriteLine("\n");
            }
        }
        static void ParseFile(string path, ref Trie<WordAndFrequency> trie, ref List<string> preds)
        {
            using (var reader = new StreamReader(path))
            {
                int dictionaryBorder;
                int InputsBorder;

                bool dictionaryParsed = int.TryParse(reader.ReadLine(), out dictionaryBorder);

                if (dictionaryParsed)
                {
                    for (int i = 0; i < dictionaryBorder; i++)
                    {
                        string[] rowstring = reader.ReadLine().Split(new char[] { ' ' });

                        trie.Add(rowstring[0], new WordAndFrequency { Word = rowstring[0], Frequency = int.Parse(rowstring[1]) });
                    }

                    bool predictParsed = int.TryParse(reader.ReadLine(), out InputsBorder);

                    if (predictParsed)
                    {
                        for (int f = 0; f < InputsBorder; f++)
                        {
                            preds.Add(reader.ReadLine());
                        }
                    }
                    else throw new Exception("Unable to parse inputs border string");

                }
                else throw new Exception("Unable to parse first string");

            }
        }
    }
}
