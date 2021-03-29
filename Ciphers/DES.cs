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
        
        public string Key { get; set; }

        public DES(string key)
        {
            Key = key;
        }

        public string Encrypt(string s)
        {
            string input = AppendBits(s);
            input = PerformInitialPermutation(input);
            
            // step 3 not implemented for now

            string reducedKey = PerformKeyPermutation();
            string leftKeyBits = reducedKey.Substring(0, 28);
            string rightKeyBits = reducedKey.Substring(28, 28);

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
        
        private string PerformKeyPermutation()
        {
            StringBuilder builder = new StringBuilder();

            foreach (int pos in PermutedChoice)
            {
                builder.Append(Key[pos - 1]);
            }

            return builder.ToString();
        }
    }
}