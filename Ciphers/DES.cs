using System.Text;

namespace WPFCiphers.Ciphers
{
    public class DES : Cipher
    {
        public string Key { get; set; }

        public DES(string key)
        {
            Key = key;
        }

        public string Encrypt(string s)
        {
            string adjustedInput = AppendBits(s);
            throw new System.NotImplementedException();
        }

        public string Decrypt(string s)
        {
            throw new System.NotImplementedException();
        }
        
        private string AppendBits(string s)
        {
            if (s.Length % 64 == 0) return s;

            StringBuilder builder = new StringBuilder(s);
            
            builder.Append('1');
            while (builder.Length % 64 != 0)
            {
                builder.Append('0');
            }

            return builder.ToString();
        }
    }
}