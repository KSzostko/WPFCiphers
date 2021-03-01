using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    public class RailFence : Cipher
    {
        public int Key { get; set; }

        public RailFence(int key)
        {
            this.Key = key;
        }

        public string Decrypt(string s)
        {
            if (Key == 1) return s;
            
            char[,] rails = GenerateRails(new string('*', s.Length));

            int current = 0;
            char[] decryptedWord = new char[s.Length];

            for (int row = 0; row < rails.GetLength(0); row++)
            {
                for (int col = 0; col < rails.GetLength(1); col++)
                {
                    if (rails[row, col] == '*')
                    {
                        decryptedWord[col] = s[current];
                        current++;
                    }
                }
            }

            return new string(decryptedWord);
        }

        public string Encrypt(string s)
        {
            if (Key == 1) return s;
            
            char[,] rails = GenerateRails(s);

            return BuildWord(rails);
        }

        private char[,] GenerateRails(string s)
        {
            char[,] rails = InitRails(s);

            bool increase = false;
            int row = 0, col = 0;

            foreach (char letter in s)
            {
                if (OnArrayBound(row))
                {
                    increase = !increase;
                }

                rails[row, col] = letter;

                row = NextRow(row, increase);
                col++;
            }

            return rails;
        }

        private string BuildWord(char[,] rails)
        {
            StringBuilder builder = new StringBuilder();
            foreach(char letter in rails)
            {
                if (letter != '\n') builder.Append(letter);
            }

            return builder.ToString();
        }

        private char[,] InitRails(string s)
        {
            char[,] rails = new char[Key, s.Length];

            for(int i = 0; i < rails.GetLength(0); i++)
            {
                for(int j = 0; j < rails.GetLength(1); j++)
                {
                    rails[i, j] = '\n';
                }
            }

            return rails;
        }

        private bool OnArrayBound(int row)
        {
            return row == 0 || row == Key - 1;
        }

        private int NextRow(int row, bool increase)
        {
            if (increase) return row + 1;

            return row - 1;
        }
    }
}
