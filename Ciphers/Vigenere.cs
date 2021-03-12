namespace WPFCiphers.Ciphers
{
    public class Vigenere : Cipher
    {
        private static readonly int ALPHABET_LETTERS_COUNT = 26;
        
        public string Key { get; set; }

        public Vigenere(string key)
        {
            Key = key.ToUpper();
        }
        public string Encrypt(string s)
        {
            s = s.ToUpper();
            string adjustedKey = AdjustKeyToWord(s);
            string encrypted = "";

            for (int i = 0; i < s.Length; i++)
            {
                int letterVal = (s[i] + adjustedKey[i]) % ALPHABET_LETTERS_COUNT;
                letterVal += 'A';

                encrypted += (char) letterVal;
            }
            
            return encrypted;
        }

        public string Decrypt(string s)
        {
            s = s.ToUpper();
            string adjustedKey = AdjustKeyToWord(s);
            string decrypted = "";

            for (int i = 0; i < s.Length; i++)
            {
                int letterVal = (s[i] - adjustedKey[i] + ALPHABET_LETTERS_COUNT) % ALPHABET_LETTERS_COUNT;
                letterVal += 'A';

                decrypted += (char) letterVal;
            }

            return decrypted;
        }
        
        private string AdjustKeyToWord(string s)
        {
            if (Key.Length >= s.Length) return Key;
            
            string newKey = Key;
            int current = 0;

            while (newKey.Length < s.Length)
            {
                newKey += Key[current];
                current = current == Key.Length - 1 ? 0 : current + 1;
            }

            return newKey;
        }
    }
}