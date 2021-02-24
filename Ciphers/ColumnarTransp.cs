using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    class ColumnarTransposition : Cipher
    {
        public int[] Key { get; set; }

        public ColumnarTransposition(int[] key)
        {
            this.Key = key;
        }

        public string Decrypt(string s)
        {
            // kolumny z literami
            List<List<char>> columns = createColumns(s);

            string decrypted_word = "";
            int column_count = 0;
            int column_current = Key[column_count] - 1;
            for (int i = 0; i < s.Length; i++)
            {
                if (columns[column_current].Any())
                {
                    char character = columns[column_current].First();
                    decrypted_word += character;
                    columns[column_current].Remove(character);
                }
                else
                {
                    decrypted_word += '-';
                    i--;
                }

                column_count = (column_count + 1) % Key.Max();
                column_current = Key[column_count] - 1;
            }

            return decrypted_word;
        }

        public string Encrypt(string s)
        {
            // ilość kolumn
            int lenght = this.Key.Max();

            // lista kolumn
            List<List<char>> columns = new List<List<char>>();
            for (int i = 0; i < lenght; i++)
            {
                List<char> new_column = new List<char>();
                columns.Add(new_column);
            }

            //słowo do kolumn
            int column_count = 0;
            int column_current = Key[column_count] - 1;
            for (int i = 0; i < s.Length; i++)
            {
                columns[column_current].Add(s[i]);

                column_count = (column_count + 1) % Key.Max();
                column_current = Key[column_count] - 1;
            }


            string encrypted_word = "";
            column_current = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (columns[column_current].Any())
                {

                    char character = columns[column_current].First();
                    if (character != '-')
                        encrypted_word += character;
                    columns[column_current].Remove(character);
                }
                else
                {
                    i--;
                }
                column_current = (column_current + 1) % Key.Max();
            }

            return encrypted_word;
        }

        private List<List<char>> createColumns(string s)
        {
            // ilość kolumn
            int lenght = this.Key.Max();

            // lista kolumn
            List<List<char>> columns = new List<List<char>>();
            for (int i = 0; i < lenght; i++)
            {
                List<char> new_column = new List<char>();
                columns.Add(new_column);
            }

            for (int i = 0; i < s.Length; i++)
            {
                int num = i % lenght;
                columns[num].Add(s[i]);
            }

            return columns;
        }
    }
}
