using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    public class ColumnarTransposition : Cipher
    {
        public int[] Key { get; set; }

        public ColumnarTransposition(int[] key)
        {
            this.Key = key;
        }
        
        public string Encrypt(string text)
        {
            string output = "";
            //jaką długość będzie mieć najkrótsza kolumna
            int minColLength = (int)Math.Floor((decimal)text.Length / Key.Length);
            //jaką długość będzie mieć najkrótszy wiersz
            int lastRowLength = text.Length % Key.Length;

            for (int i = 0; i <= minColLength; i++)
            {
                foreach (int colNr in Key)
                {
                    if (i == minColLength)
                    {
                        if (colNr <= lastRowLength)
                            output += text[(colNr + ((i) * Key.Length)) - 1];
                    }
                    else
                        output += text[(colNr + ((i) * Key.Length)) - 1];
                }
            }

            return output;
        }


        public string Decrypt(string text)
        {
            char[] output = new char[text.Length];
            //jaką długość będzie mieć najkrótsza kolumna
            int minColLength = (int)Math.Floor((decimal)text.Length / Key.Length);
            //jaką długość będzie mieć najkrótszy wiersz
            int lastRowLength = text.Length % Key.Length;
            // 
            int row = 0;

            for (int i = 0; i <= minColLength; i++)
            {
                for (int j = 0; j < Key.Length; j++)
                {
                    if (i == minColLength)
                    {
                        if (Key[j] <= lastRowLength)
                        {
                            output[(Key[j]) + ((i) * Key.Length) - 1] = text[row + i * Key.Length];
                            row++;
                        }
                    }
                    else
                        output[(Key[j]) + ((i) * Key.Length) - 1] = text[j + i * Key.Length];
                }
            }

            return new string(output);
        }
    }
}
