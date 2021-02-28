using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    class ColumnarTransposition : Cipher
    {
        public List<int> key { get; set; }

        public ColumnarTransposition(List<int> key)
        {
            this.key = key;
        }


        public string Encrypt(string text)
        {
            string output = "";
            //jaką długość będzie mieć najkrótsza kolumna
            int column_lenght = (int)Math.Floor((decimal)text.Length / key.Count);
            //ile będzie kolumn z maksymalną długością, jak 0 to wszystkie
            int column_full = text.Length % key.Count;

            for (int i = 0; i <= column_lenght; i++)
            {
                for (int j = 0; j < key.Count; j++)
                {
                    if (i == column_lenght)
                    {
                        if (key[j] <= column_full)
                            output += text[(key[j] + ((i) * key.Count)) - 1];
                    }
                    else
                        output += text[(key[j] + ((i) * key.Count)) - 1];
                }
            }

            return output;
        }


        public string Decrypt(string text)
        {
            char[] output = new char[text.Length];
            //jaką długość będzie mieć najkrótsza kolumna
            int column_lenght = (int)Math.Floor((decimal)text.Length / key.Count);
            //ile będzie kolumn z maksymalną długością, jak 0 to wszystkie
            int column_full = text.Length % key.Count;
            // 
            int row = 0;

            for (int i = 0; i <= column_lenght; i++)
            {
                for (int j = 0; j < key.Count; j++)
                {
                    if (i == column_lenght)
                    {
                        if (key[j] <= column_full)
                        {
                            output[(key[j]) + ((i) * key.Count) - 1] = text[row + i * key.Count];
                            row++;
                        }
                    }
                    else
                        output[(key[j]) + ((i) * key.Count) - 1] = text[j + i * key.Count];
                }
            }

            return new string(output);
        }
    }
}
