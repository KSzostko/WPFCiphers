﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WPFCiphers.Ciphers
{
    public class DES : Cipher
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

        public string Encrypt(string s)
        {
            string input = AppendBits(s);
            input = PerformInitialPermutation(input);
            DivideInputBits(input);

            string reducedKey = PerformKeyPermutation();
            string leftKeyBits = reducedKey.Substring(0, 28);
            string rightKeyBits = reducedKey.Substring(28, 28);
            ShiftKeyBits(leftKeyBits, rightKeyBits);

            CreatePermutedKeys();

            throw new System.NotImplementedException();
        }

        public string Decrypt(string s)
        {
            throw new System.NotImplementedException();
        }
        
        private string AppendBits(string s)
        {
            if (s.Length % BlockSize == 0) return s;

            StringBuilder builder = new StringBuilder(s);
            
            builder.Append('1');
            while (builder.Length % BlockSize != 0)
            {
                builder.Append('0');
            }

            return builder.ToString();
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
    }
}