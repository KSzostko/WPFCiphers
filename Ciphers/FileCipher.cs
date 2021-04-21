namespace WPFCiphers.Ciphers
{
    public interface FileCipher
    {
        void EncryptFile(string filename);
        void DecryptFile(string filename);
    }
}