using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFCiphers.Ciphers
{
    public class MatrixTransp : Cipher
    {
        private int[] rowsOrder;
        public string Key { get; set; }

        public MatrixTransp(string key)
        {
            Key = key;
            rowsOrder = new int[key.Length];
            
            CalculateRowsOrder();
        }
        
        public string Encrypt(string s)
        {
            char[,] letters = GenerateLettersArray(s);

            return BuildWord(letters);
        }

        public string Decrypt(string s)
        {
            char[,] letters = PrepareForRowsAdjusting(s);
            char[,] finalWord = DecryptRowsOrder(letters);

            StringBuilder builder = new StringBuilder();
            
            for (int col = 0; col < letters.GetLength(1); col++)
            {
                for (int row = 0; row < letters.GetLength(0); row++)
                {
                    builder.Append(finalWord[row, col]);
                }
            }

            return builder.ToString();
        }

        private void CalculateRowsOrder()
        {
            char[] chars = Key.ToArray();
            Array.Sort(chars);

            Dictionary<char, int> occurrences = InitOccurrences(chars);
            
            for (int i = 0; i < rowsOrder.Length; i++)
            {
                rowsOrder[i] = Array.IndexOf(chars, Key[i]) + occurrences[Key[i]];
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
            for (int col = 0; col < letters.GetLength(1); col++)
            {
                for (int row = 0; row < letters.GetLength(0); row++)
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
        
        private string BuildWord(char[,] letters)
        {
            char[,] finalWord = AdjustRowsOrder(letters);
            
            StringBuilder builder = new StringBuilder();
            for (int row = 0; row < finalWord.GetLength(0); row++)
            {
                if (row != 0) builder.Append(' ');
                
                int col = 0;
                while (col < finalWord.GetLength(1) && IsLetter(finalWord[row, col]))
                {
                    builder.Append(finalWord[row, col]);
                    col++;
                }
            }

            return builder.ToString();
        }

        private char[,] AdjustRowsOrder(char[,] letters)
        {
            char[,] finalWord = new char[letters.GetLength(0), letters.GetLength(1)];
            for (int i = 0; i < rowsOrder.Length; i++)
            {
                int currentRow = rowsOrder[i];
                for (int col = 0; col < letters.GetLength(1); col++)
                {
                    finalWord[currentRow, col] = letters[i, col];
                }
            }

            return finalWord;
        }
        
        private char[,] PrepareForRowsAdjusting(string s)
        {
            int rows = s.Length / Key.Length;
            if (s.Length % Key.Length != 0) rows++;
            char[,] letters = new char[Key.Length, rows];

            int currRow = 0, currCol = 0;
            foreach (char letter in s)
            {
                if (letter == ' ')
                {
                    currRow++;
                    currCol = 0;
                }
                else
                {
                    letters[currRow, currCol] = letter;
                    currCol++;
                }
            }

            return letters;
        }
        
        private char[,] DecryptRowsOrder(char[,] letters)
        {
            char[,] finalWord = new char[letters.GetLength(0), letters.GetLength(1)];
            for (int i = 0; i < rowsOrder.Length; i++)
            {
                for (int col = 0; col < letters.GetLength(1); col++)
                {
                    finalWord[i, col] = letters[rowsOrder[i], col];
                }
            }

            return finalWord;
        }
    }
}