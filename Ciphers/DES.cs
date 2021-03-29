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
            throw new System.NotImplementedException();
        }

        public string Decrypt(string s)
        {
            throw new System.NotImplementedException();
        }
    }
}