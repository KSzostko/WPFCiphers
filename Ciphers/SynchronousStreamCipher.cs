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
        public List<bool> key { get; set; }
        public BitArray bit_key { get; set; }
 
        public SynchronousStreamCipher(List<bool> key)
        {
            this.key = key;
            this.bit_key = boolToBinary(key);
        }

        public BitArray Decrypt(string file_input)
        {
            BitArray bit_file = GetFileBits(file_input);
            BitArray output = new BitArray(bit_file.Count);

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
            

            saveDecrypted(output);

            return output;
        }

        public BitArray Encrypt(string file_input)
        {
            BitArray bit_file = GetFileBits(file_input);
            BitArray output = new BitArray(bit_file.Count);

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