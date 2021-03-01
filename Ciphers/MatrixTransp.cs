using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFCiphers.Ciphers
{
    public class MatrixTransp : Cipher
    {
        private int[] colsOrder;
        public string Key { get; set; }

        public MatrixTransp(string key)
        {
            Key = key;
            colsOrder = new int[key.Length];
            
            CalculateRowsOrder();
        }
        
        public string Encrypt(string s)
        {
            char[,] letters = GenerateLettersArray(s);
            
            throw new System.NotImplementedException();
        }

        public string Decrypt(string s)
        {
            throw new System.NotImplementedException();
        }

        private void CalculateRowsOrder()
        {
            char[] chars = Key.ToArray();
            Array.Sort(chars);

            Dictionary<char, int> occurrences = InitOccurrences(chars);
            
            for (int i = 0; i < colsOrder.Length; i++)
            {
                colsOrder[i] = Array.IndexOf(chars, Key[i]) + occurrences[Key[i]];
                occurrences[Key[i]]++;
            }
        }

        private Dictionary<char, int> InitOccurrences(char[] chars)
        {
            Dictionary<char, int> occurences = new Dictionary<char, int>();
            foreach (char ch in chars)
            {
                occurences[ch] = 0;
            }

            return occurences;
        }
        
        private char[,] GenerateLettersArray(string s)
        {
            int rows = s.Length / Key.Length;
            if (s.Length % Key.Length != 0) rows++;
            char[,] letters = new char[Key.Length, rows];

            int current = 0;
            for (int row = 0; row < letters.GetLength(0); row++)
            {
                for (int col = 0; col < letters.GetLength(1); col++)
                {
                    while (current != s.Length && !IsLetter(s[current])) current++;
                    if (current == s.Length) break;
                    
                    letters[row, col] = s[current];
                    current++;
                }
            }

            return letters;
        }
        
        private bool IsLetter(char ch)
        {
            return ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z';
        }
    }
}