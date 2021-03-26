using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFCiphers.Generators;

namespace WPFCiphers.Ciphers
{
    public class SynchronousStreamCipher
    {
        //lista boolowska jako klucz
        public List<bool> key { get; set; }
        public BitArray bit_key { get; set; }

        // LFSR jest jedno bo z nieznianych mi powodów przy dekodowaniu
        // czyli kiedy tworzę drugi LFSR to nie chce mi generować klucza 
        public SynchronousStreamCipher(List<bool> key)
        {
            this.key = key;
            this.bit_key = boolToBinary(key);
        }

        public BitArray Decrypt(string file_input)
        {
            BitArray bit_file = GetFileBits(file_input);
            BitArray output = new BitArray(bit_file.Count);

            //XOR-owanie klucza i pliku
            int file_iter = 0;
            int key_iter = 0;
            while (file_iter < bit_file.Count)
            {
                if (key_iter == key.Count)
                    key_iter = 0;

                output[file_iter] = bit_file[file_iter] ^ key[key_iter];

                file_iter++;
                key_iter++;
            }
            /*
            // do sprawdzenia czy działa, można usunąć
            Console.Write("key: ");
            for (int i = 0; i < bit_key.Count; i++)
            {
                if (bit_key[i] == true)
                    Console.Write('1');
                else
                    Console.Write('0');
            }
            Console.Write("\nfil: ");
            for (int i = 0; i < bit_file.Count; i++)
            {
                if (bit_file[i] == true)
                    Console.Write('1');
                else
                    Console.Write('0');
            }
            Console.Write("\nout: ");
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] == true)
                    Console.Write('1');
                else
                    Console.Write('0');
            }
            Console.Write('\n');
            */

            saveDecrypted(output);

            return output;
        }

        public BitArray Encrypt(string file_input)
        {
            BitArray bit_file = GetFileBits(file_input);
            BitArray output = new BitArray(bit_file.Count);

            //XOR-owanie klucza i pliku
            int file_iter = 0;
            int key_iter = 0;
            while (file_iter < bit_file.Count)
            {
                if (key_iter == key.Count)
                    key_iter = 0;

                output[file_iter] = bit_file[file_iter] ^ key[key_iter];

                file_iter++;
                key_iter++;
            }
            /*
            // do sprawdzenia czy działa, można usunąć
            Console.Write("key: ");
            for (int i = 0; i < bit_key.Count; i++)
            {
                if (bit_key[i] == true)
                    Console.Write('1');
                else
                    Console.Write('0');
            }
            Console.Write("\nfil: ");
            for (int i = 0; i < bit_file.Count; i++)
            {
                if (bit_file[i] == true)
                    Console.Write('1');
                else
                    Console.Write('0');
            }
            Console.Write("\nout: ");
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i] == true)
                    Console.Write('1');
                else
                    Console.Write('0');
            }
            Console.Write('\n');
            */
            saveEnrypted(output);

            return output;
        }

        public BitArray boolToBinary(List<bool> key)
        {
            BitArray output = new BitArray(key.Count);

            for (int i = 0; i < key.Count; i++)
            {
                if (key[i] == true)
                    output[i] = true;
                else
                    output[i] = false;
            }

            return output;
        }

        private BitArray GetFileBits(String filename)
        {
            byte[] bytes = File.ReadAllBytes(filename);
            return new BitArray(bytes);
        }

        private void saveDecrypted(BitArray input)
        {
            byte[] bytes = new byte[input.Length / 8 + (input.Length % 8 == 0 ? 0 : 1)];
            input.CopyTo(bytes, 0);
            File.WriteAllBytes("decrypted.bin", bytes);
        }

        private void saveEnrypted(BitArray input)
        {
            byte[] bytes = new byte[input.Length / 8 + (input.Length % 8 == 0 ? 0 : 1)];
            input.CopyTo(bytes, 0);
            File.WriteAllBytes("encrypted.bin", bytes);
        }
    }
}