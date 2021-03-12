using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class VinegereTest
    {
        [TestMethod]
        public void ReturnsEncryptedWordForKeyMatchingWordLength()
        {
            string input = "CRYPTOGRAPHY";
            string expected = "DICPDPXVAZIP";

            Vigenere vigenere = new Vigenere("BREAKBREAKBR");

            string encrypted = vigenere.Encrypt(input);
            Assert.AreEqual(expected, encrypted);
        }
        
        [TestMethod]
        public void ReturnsDecryptedWordForKeyMatchingWordLength()
        {
            string input = "DICPDPXVAZIP";
            string expected = "CRYPTOGRAPHY";

            Vigenere vigenere = new Vigenere("BREAKBREAKBR");

            string decrypted = vigenere.Decrypt(input);
            Assert.AreEqual(expected, decrypted);
        }

        [TestMethod]
        public void ReturnsEncryptedWordForKeyShorterThanWord()
        {
            string input = "CRYPTOGRAPHY";
            string expected = "DICPDPXVAZIP";

            Vigenere vigenere = new Vigenere("BREAK");

            string encrypted = vigenere.Encrypt(input);
            Assert.AreEqual(expected, encrypted);
        }
        
        [TestMethod]
        public void ReturnsDecryptedWordForKeyShorterThanWord()
        {
            string input = "DICPDPXVAZIP";
            string expected = "CRYPTOGRAPHY";

            Vigenere vigenere = new Vigenere("BREAK");

            string decrypted = vigenere.Decrypt(input);
            Assert.AreEqual(expected, decrypted);
        }
        
        [TestMethod]
        public void ReturnsEncryptedWordForKeyMatchingWordLengthWithSmallLetters()
        {
            string input = "cryptography";
            string expected = "DICPDPXVAZIP";

            Vigenere vigenere = new Vigenere("breakbreakbr");

            string encrypted = vigenere.Encrypt(input);
            Assert.AreEqual(expected, encrypted);
        }
        
        [TestMethod]
        public void ReturnsDecryptedWordForKeyMatchingWordLengthWithSmallLetters()
        {
            string input = "dicpdpxvazip";
            string expected = "CRYPTOGRAPHY";

            Vigenere vigenere = new Vigenere("breakbreakbr");

            string decrypted = vigenere.Decrypt(input);
            Assert.AreEqual(expected, decrypted);
        }

        [TestMethod]
        public void ReturnsEncryptedWordForKeyShorterThanWordWithSmallLetters()
        {
            string input = "cryptography";
            string expected = "DICPDPXVAZIP";

            Vigenere vigenere = new Vigenere("break");

            string encrypted = vigenere.Encrypt(input);
            Assert.AreEqual(expected, encrypted);
        }
        
        [TestMethod]
        public void ReturnsDecryptedWordForKeyShorterThanWordWithSmallLetters()
        {
            string input = "dicpdpxvazip";
            string expected = "CRYPTOGRAPHY";

            Vigenere vigenere = new Vigenere("break");

            string decrypted = vigenere.Decrypt(input);
            Assert.AreEqual(expected, decrypted);
        }
    }
}