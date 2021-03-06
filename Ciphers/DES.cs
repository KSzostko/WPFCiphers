﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WPFCiphers.Ciphers
{
    public class DES : FileCipher
    {
        private static readonly int BlockSize = 64;
        private static readonly int[,] InitialPermutation = {
            {58, 50, 42, 34, 26, 18, 10, 2},
            {60, 52, 44, 36, 28, 20, 12, 4},
            {62, 54, 46, 38, 30, 22, 14, 6},
            {64, 56, 48, 40, 32, 24, 16, 8},
            {57, 49, 41, 33, 25, 17, 9, 1},
            {59, 51, 43, 35, 27, 19, 11, 3},
            {61, 53, 45, 37, 29, 21, 13, 5},
            {63, 55, 47, 39, 31, 23, 15, 7}
        };
        private static readonly int[,] PermutedChoice = {
            {57, 49, 41, 33, 25, 17, 9},
            {1, 58, 50, 42, 34, 26, 18},
            {10, 2, 59, 51, 43, 35, 27},
            {19, 11, 3, 60, 52, 44, 36},
            {63, 55, 47, 39, 31, 23, 15},
            {7, 62, 54, 46, 38, 30, 22},
            {14, 6, 61, 53, 45, 37, 29},
            {21, 13, 5, 28, 20, 12, 4}
        };
        private static readonly int[] LeftShift = {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};
        private static readonly int[,] PermutedChoice2 = {
            {14, 17, 11, 24, 1, 5},
            {3, 28, 15, 6, 21, 10},
            {23, 19, 12, 4, 26, 8},
            {16, 7, 27, 20, 13, 2},
            {41, 52, 31, 37, 47, 55},
            {30, 40, 51, 45, 33, 48},
            {44, 49, 39, 56, 34, 53},
            {46, 42, 50, 36, 29, 32}
        };
        private static readonly int[,] ExtenstionArray = {
            {32, 1, 2, 3, 4, 5},
            {4, 5, 6, 7, 8, 9},
            {8, 9, 10, 11, 12, 13},
            {12, 13, 14, 15, 16, 17},
            {16, 17, 18, 19, 20, 21},
            {20, 21, 22, 23, 24, 25},
            {24, 25, 26, 27, 28, 29},
            {28, 29, 30, 31, 32, 1}
        };
        private static readonly int[][,] RightBitsPermutations =
        {
            new[,]
            {
                {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },
            new[,]
            {
                {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },
            new[,]
            {
                {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },
            new[,]
            {
                {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },
            new[,]
            {
                {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },
            new[,]
            {
                {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },
            new[,]
            {
                {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },
            new[,]
            {
                {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }
        };
        private static readonly int[,] PermutationFunction = {
            {16, 7, 20, 21},
            {29, 12, 28, 17},
            {1, 15, 23, 26},
            {5, 18, 31, 10},
            {2, 8, 24, 14},
            {32, 27, 3, 9},
            {19, 13, 30, 6},
            {22, 11, 4, 25}
        };
        private static readonly int[,] ReverseInitialPermutation = {
            {40, 8, 48, 16, 56, 24, 64, 32},
            {39, 7, 47, 15, 55, 23, 63, 31},
            {38, 6, 46, 14, 54, 22, 62, 30},
            {37, 5, 45, 13, 53, 21, 61, 29},
            {36, 4, 44, 12, 52, 20, 60, 28},
            {35, 3, 43, 11, 51, 19, 59, 27},
            {34, 2, 42, 10, 50, 18, 58, 26},
            {33, 1, 41, 9, 49, 17, 57, 25}
        };

        private List<string> _rightInputBits;
        private List<string> _leftInputBits;

        private List<string> _leftKeyShiftedBits;
        private List<string> _rightKeyShiftedBits;

        private List<string> _permutedKeys;
        
        public string Key { get; set; }

        public DES(string key)
        {
            Key = key;
        }

        public void EncryptFile(string filename)
        {
            string extension = Path.GetExtension(filename);
            extension = ConvertTextToBinaryString(extension);

            BitArray bit_file = GetFileBits(filename);
            string input = ConvertBitArrayToString(bit_file);

            input = AppendBitsWithExtension(input, extension);

            string encrypted = Encrypt(input);
            BitArray output = ConvertStringToBitArray(encrypted);

            SaveFile(output, ".bin", 'e');
        }

        public void DecryptFile(string filename)
        {
            BitArray bit_file = GetFileBits(filename);
            string input = ConvertBitArrayToString(bit_file);

            string decrypted = Decrypt(input);


            string extension = DecryptExtension(decrypted);
            extension = ConvertBinaryToString(extension);

            decrypted = RemoveAppendedBitsWithExtension(decrypted);

            BitArray output = ConvertStringToBitArray(decrypted);
            SaveFile(output, extension, 'd');
        }

     
        private BitArray GetFileBits(String filename)
        {
            byte[] bytes = File.ReadAllBytes(filename);
            return new BitArray(bytes);
        }

       
        private string ConvertTextToBinaryString(string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in input.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

      
        private string ConvertBinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

       
        private string ConvertBitArrayToString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

       
        private BitArray ConvertStringToBitArray(string input)
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

        
        private string AppendBitsWithExtension(string input, string extension)
        {
            StringBuilder builder = new StringBuilder(input);
            
            builder.Append('1');
            while (builder.Length % BlockSize != 0)
            {
                builder.Append('0');
            }

            
            int bitstart = BlockSize - extension.Length;
            int extensionbit = 0;
            for (int i = 0; i < BlockSize; i++)
            {
                if (i >= bitstart)
                {
                    builder.Append(extension[extensionbit]);
                    extensionbit++;
                }
                else
                    builder.Append('0');
            }

            return builder.ToString();
        }

        
        private void SaveFile(BitArray input, string extension, char type)
        {
            if (type == 'e')
            {
                byte[] bytes = new byte[input.Length / 8 + (input.Length % 8 == 0 ? 0 : 1)];
                input.CopyTo(bytes, 0);
                string filename = "encrypted-DES" + extension;
                File.WriteAllBytes(filename, bytes);
            }
            else
            {
                byte[] bytes = new byte[input.Length / 8 + (input.Length % 8 == 0 ? 0 : 1)];
                input.CopyTo(bytes, 0);
                string filename = "decrypted-DES" + extension;
                File.WriteAllBytes(filename, bytes);
            }
        }

        
        private string DecryptExtension(string input)
        {
            string dotCharInBinaryString = "00101110";
            int at = input.LastIndexOf(dotCharInBinaryString);
            string output = input.Substring(at);

            return output;
        }

        
        private string RemoveAppendedBitsWithExtension(string s)
        {
            s = s.Substring(0, s.Length - BlockSize);

            int currentIndex = s.Length - 1;

            while (s[currentIndex] == '0') currentIndex--;

            string res = s.Remove(currentIndex);

            return res;
        }

        private string Encrypt(string s)
        {
            string input = PerformInitialPermutation(s);
            DivideInputBits(input);

            string reducedKey = PerformKeyPermutation();
            string leftKeyBits = reducedKey.Substring(0, 28);
            string rightKeyBits = reducedKey.Substring(28, 28);
            ShiftKeyBits(leftKeyBits, rightKeyBits);

            CreatePermutedKeys();

            ComputeInputBits();
            string res = MergeAllBits();

            return res;
        }

        private string Decrypt(string s)
        {
            string input = PerformInitialPermutation(s);
            DivideInputBits(input);

            string reducedKey = PerformKeyPermutation();
            string leftKeyBits = reducedKey.Substring(0, 28);
            string rightKeyBits = reducedKey.Substring(28, 28);
            ShiftKeyBits(leftKeyBits, rightKeyBits);
            
            CreatePermutedKeys();
            _permutedKeys.Reverse();

            ComputeInputBits();
            string res = MergeAllBits();
            
            return res;
        }

        private string PerformInitialPermutation(string s)
        {
            StringBuilder builder = new StringBuilder();
            int blocksCount = s.Length / BlockSize;

            for (int currentBlock = 0; currentBlock < blocksCount; currentBlock++)
            {
                foreach (int pos in InitialPermutation)
                {
                    int inputIndex = pos - 1 + currentBlock * BlockSize;
                    builder.Append(s[inputIndex]);
                }
            }

            return builder.ToString();
        }

        private void DivideInputBits(string input)
        {
            _leftInputBits = new List<string>();
            _rightInputBits = new List<string>();
            int prevIndex = 0;
            
            while(prevIndex != input.Length)
            {
                string left = input.Substring(prevIndex, 32);
                string right = input.Substring(prevIndex + 32, 32);
                _leftInputBits.Add(left);
                _rightInputBits.Add(right);

                prevIndex += BlockSize;
            }
        }
        
        private string PerformKeyPermutation()
        {
            StringBuilder builder = new StringBuilder();

            foreach (int pos in PermutedChoice)
            {
                builder.Append(Key[pos - 1]);
            }

            return builder.ToString();
        }

        private void ShiftKeyBits(string leftKeyBits, string rightKeyBits)
        {
            _leftKeyShiftedBits = new List<string>();
            _rightKeyShiftedBits = new List<string>();

            string currentLeftShiftedBits = leftKeyBits;
            string currentRightShiftedBits = rightKeyBits;
            foreach (int shiftCount in LeftShift)
            {
                currentLeftShiftedBits = Shift(currentLeftShiftedBits, shiftCount);
                currentRightShiftedBits = Shift(currentRightShiftedBits, shiftCount);
                
                _leftKeyShiftedBits.Add(currentLeftShiftedBits);
                _rightKeyShiftedBits.Add(currentRightShiftedBits);
            }
        }

        private string Shift(string s, int count)
        {
            return s.Remove(0, count) + s.Substring(0, count);
        }

        private void CreatePermutedKeys()
        {
            _permutedKeys = new List<string>();

            for (int i = 0; i < _leftKeyShiftedBits.Count; i++)
            {
                string joinedBits = _leftKeyShiftedBits[i] + _rightKeyShiftedBits[i];
                string permutedBits = PerformPc2(joinedBits);
                
                _permutedKeys.Add(permutedBits);
            }
        }

        private string PerformPc2(string key)
        {
            StringBuilder builder = new StringBuilder();

            foreach (int pos in PermutedChoice2)
            {
                builder.Append(key[pos - 1]);
            }

            return builder.ToString();
        }

        private void ComputeInputBits()
        {
            for (int i = 0; i < _rightInputBits.Count; i++)
            {
                string currentRightBits = _rightInputBits[i];
                string currentLeftBits = _leftInputBits[i];

                for (int key = 0; key < 16; key++)
                {
                    string extended = PerformBitsExtension(currentRightBits);
                    string xoredBits = PerformXorWithKey(extended, key);

                    string[] dataPositions = DivideRightInputBits(xoredBits);
                    string mergedBits = CalculatePositions(dataPositions);
                    string permutedBits = PerformPermutationFunction(mergedBits);

                    string oldRightBits = currentRightBits;
                    currentRightBits = XorLeftAndRightInputBits(currentLeftBits, permutedBits);
                    currentLeftBits = oldRightBits;
                }

                _rightInputBits[i] = currentRightBits;
                _leftInputBits[i] = currentLeftBits;
            }
        }

        private string PerformBitsExtension(string bits)
        {
            StringBuilder builder = new StringBuilder();

            foreach (int pos in ExtenstionArray)
            {
                builder.Append(bits[pos - 1]);
            }

            return builder.ToString();
        }

        private string PerformXorWithKey(string bits, int index)
        {
            StringBuilder builder = new StringBuilder();
            string key = _permutedKeys[index];

            for (int i = 0; i < bits.Length; i++)
            {
                bool res = key[i] != bits[i];
                char bit = res ? '1' : '0';
                builder.Append(bit);
            }

            return builder.ToString();
        }

        private string[] DivideRightInputBits(string bits)
        {
            string[] res = new string[8];
            int resCurrent = 0;
            
            int prevIndex = 0;
            while(prevIndex != bits.Length)
            {
                string fragment = bits.Substring(prevIndex, 6);
                res[resCurrent] = fragment;
                resCurrent++;

                prevIndex += 6;
            }

            return res;
        }

        private string CalculatePositions(string[] dataPositions)
        {
            StringBuilder builder = new StringBuilder();
            
            for (int i = 0; i < dataPositions.Length; i++)
            {
                int row = CalculateRow(dataPositions[i]);
                int column = CalculateColumn(dataPositions[i]);

                string binaryNumber = GetValueAtPosition(i, row, column);
                builder.Append(binaryNumber);
            }
            
            return builder.ToString();
        }

        private int CalculateRow(string data)
        {
            StringBuilder rowInBinary = new StringBuilder();
            rowInBinary.Append(data[0]);
            rowInBinary.Append(data[data.Length - 1]);
            
            return ConvertBitsToDecimal(rowInBinary.ToString());
        }
        
        private int CalculateColumn(string data)
        {
            string colInBinary = data.Substring(1, 4);

            return ConvertBitsToDecimal(colInBinary);
        }

        private int ConvertBitsToDecimal(string bits)
        {
            int multiplier = 1;
            int res = 0;
            
            foreach (int asciiValue in bits.Reverse())
            {
                int bit = asciiValue - '0';
                res += bit * multiplier;

                multiplier *= 2;
            }
            return res;
        }

        private string GetValueAtPosition(int iteration, int row, int col)
        {
            int number = RightBitsPermutations[iteration][row, col];

            return ConvertDecimalToBits(number);
        }

        private string ConvertDecimalToBits(int number)
        {
            string res =  Convert.ToString(number, 2);
            if (res.Length < 4)
            {
                while (res.Length < 4)
                {
                    res = "0" + res;
                }
            }

            return res;
        }

        private string PerformPermutationFunction(string bits)
        {
            StringBuilder builder = new StringBuilder();

            foreach (int pos in PermutationFunction)
            {
                builder.Append(bits[pos - 1]);
            }

            return builder.ToString();
        }

        private string XorLeftAndRightInputBits(string leftBits, string rightBits)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < rightBits.Length; i++)
            {
                bool res = leftBits[i] != rightBits[i];
                char bit = res ? '1' : '0';
                builder.Append(bit);
            }

            return builder.ToString();
        }

        private string MergeAllBits()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < _leftInputBits.Count; i++)
            {
                string merged = _rightInputBits[i] + _leftInputBits[i];
                string permuted = PerformReversePermutation(merged);

                builder.Append(permuted);
            }

            return builder.ToString();
        }

        private string PerformReversePermutation(string bits)
        {
            StringBuilder builder = new StringBuilder();

            foreach (int pos in ReverseInitialPermutation)
            {
                builder.Append(bits[pos - 1]);
            }

            return builder.ToString();
        }
    }
}