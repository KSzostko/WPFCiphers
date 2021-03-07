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
            string adjustedKey = AdjustKeyToWord(s);
            throw new System.NotImplementedException();
        }

        public string Decrypt(string s)
        {
            throw new System.NotImplementedException();
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