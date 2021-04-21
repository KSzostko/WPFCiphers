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

            // otrzymanie rozszerzenia 
            string temp = BitArrayToString(output);
            string extension = decryptedExtension(temp);
            int extension_lenght = extension.Length;
            extension = BinaryToString(extension);

            // zredukowanie wyjścia o rozszerzenie
            BitArray reduced = new BitArray(output.Count - extension_lenght);
            for (int i = 0; i < reduced.Count; i++)
                reduced[i] = output[i];

            saveDecrypted(reduced, extension);

            return reduced;
        }

        public BitArray Encrypt(string file_input)
        {
            BitArray bit_file = GetFileBits(file_input);

            string extension = Path.GetExtension(file_input);
            extension = stringToBinaryString(extension);
            BitArray bit_extension = stringToBitArray(extension);

            BitArray output = new BitArray(bit_file.Count + bit_extension.Count);

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

            // dodanie rozszerzenia i xorowanie
            int extension_iter = 0;
            while (extension_iter < bit_extension.Count)
            {
                if (key_iter == key.Count)
                    key_iter = 0;

                output[file_iter + extension_iter] = bit_extension[extension_iter] ^ key[key_iter];

                extension_iter++;
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

        private void saveDecrypted(BitArray input, string extension)
        {

            byte[] bytes = new byte[(input.Length) / 8 + (input.Length % 8 == 0 ? 0 : 1)];
            input.CopyTo(bytes, 0);
            File.WriteAllBytes("decrypted" + extension, bytes);
        }

        private void saveEnrypted(BitArray input)
        {
            byte[] bytes = new byte[input.Length / 8 + (input.Length % 8 == 0 ? 0 : 1)];
            input.CopyTo(bytes, 0);
            File.WriteAllBytes("encrypted.bin", bytes);
        }


        // zamiana stringa na string zer i jedynek
        private string stringToBinaryString(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        // zamiana BitArray na string
        private string BitArrayToString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        // zamiana stringa na BitArray
        private BitArray stringToBitArray(string input)
        {
            BitArray output = new BitArray(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '1')
                    output[i] = true;
                else
                    output[i] = false;
            }

            return output;
        }

        private string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        // odczytanie rozszerzenia z dekodowanej wiadomości
        private string decryptedExtension(string input)
        {
            int at = input.LastIndexOf("00101110");
            string output = input.Substring(at);

            return output;
        }
    }
}