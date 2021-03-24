using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    public class SynchronousStreamCipher : Cipher
    {
        // wielomian jako tablica intów(potęgi x)
        public int[] seed { get; set; }
        private LFSR number_generator;

        // LFSR jest jedno bo z nieznianych mi powodów przy dekodowaniu
        // czyli kiedy tworzę drugi LFSR to nie chce mi generować klucza 
        public SynchronousStreamCipher(int[] seed)
        {
            this.seed = seed;
            number_generator = new LFSR();
        }

        public string Decrypt(string text)
        {
            // klucz jako bajty
            List<byte> byte_key = getKey(text);

            //XOR-owanie klucza i textu 
            string output = "";
            for (int i = 0; i < text.Length; i++)
            {
                if ((text[i] == '0' && byte_key[i] == 0) ||
                    (text[i] == '1' && byte_key[i] == 1))
                {
                    output += '0';
                }
                else
                    output += '1';
            }

            // zamiana z binarnej do zwykłej
            output = binaryToString(output);

            // do sprawdzenia czy działa, można usunąć
            Console.Write("byte_text: ");
            Console.WriteLine(text);
            Console.Write("byte_key:  ");
            for (int i = 0; i < text.Length; i++)
                Console.Write(byte_key[i]);
            Console.Write("\n");

            return output;
        }

        public string Encrypt(string text)
        {
            // string jako bajty
            string byte_text = stringToBinary(text);
            // klucz jako bajty
            List<byte> byte_key = getKey(byte_text);

            //XOR-owanie klucza i textu 
            // trochę pokrętnie ale byte_text to string a byte_key to byte
            string output = "";
            for (int i = 0; i < byte_text.Length; i++)
            {
                if ((byte_text[i] == '0' && byte_key[i] == 0) ||
                    (byte_text[i] == '1' && byte_key[i] == 1))
                {
                    output += '0';
                }
                else
                    output += '1';
            }

            // do sprawdzenia czy działa, można usunąć
            Console.Write("byte_text: ");
            Console.WriteLine(byte_text);
            Console.Write("byte_key:  ");
            for (int i = 0; i < byte_text.Length; i++)
                Console.Write(byte_key[i]);
            Console.Write("\n");

            return output;
        }

        // do pobrania klucza
        private List<byte> getKey(string byte_text)
        {
            number_generator.StartGenerator(seed);

            List<bool> key = new List<bool>();
            List<byte> byte_key = new List<byte>();

            while (byte_key.Count() < byte_text.Length)
            {
                key = number_generator.GetSequence();
                //Console.WriteLine(key.Count); 
                // jeżeli LFSR będzie tworzone w tej funkcji to za 2 razem czyli przy dekodwaniu
                // nie zwraca klucza (długość = 0), albo Ja ślepy na problem albo coś jest nie tak
                for (int i = 0; i < key.Count; i++)
                {
                    if (key[i] == true)
                        byte_key.Add(1);
                    else
                        byte_key.Add(0);
                }
            }
            number_generator.StopGenerator();

            return byte_key;
        }

        // string zwykły na string 0 i 1
        public String stringToBinary(string text)
        {
            /*
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] buf = encoding.GetBytes(text);

            StringBuilder binaryStringBuilder = new StringBuilder();
            foreach (byte b in buf)
            {
                binaryStringBuilder.Append(Convert.ToString(b, 2));
            }

            return binaryStringBuilder.ToString();
            */

            StringBuilder sb = new StringBuilder();

            foreach (char c in text.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public String binaryToString(string text)
        {
            /*
            List<Byte> byteList = new List<Byte>();
            
            for (int i = 0; i < text.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(text.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
            */

            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < text.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(text.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }
    }
}
