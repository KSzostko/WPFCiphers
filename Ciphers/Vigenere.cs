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
            throw new System.NotImplementedException();
        }

        public string Decrypt(string s)
        {
            throw new System.NotImplementedException();
        }
    }
}