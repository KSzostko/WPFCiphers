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
        
        public string Encrypt(string Word)
        {
            throw new System.NotImplementedException();
        }

        public string Decrypt(string Word)
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
    }
}