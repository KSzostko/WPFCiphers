namespace WPFCiphers.Ciphers
{
    public class Cezar : Cipher
    {
        int cipherKey { get; set; }
        public Cezar(int key)
        {
            this.cipherKey = key;
        }
        public static char cipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {

                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);


        }


        public string Encrypt(string input)
        {
            int key = cipherKey;
            string output = string.Empty;

            foreach (char ch in input)
                output += cipher(ch, key);

            return output;
        }

        public string Decrypt(string input)
        {
            int key = 26 - cipherKey;
            string output = string.Empty;

            foreach (char ch in input)
                output += cipher(ch, key);

            return output;
        }
    }
}