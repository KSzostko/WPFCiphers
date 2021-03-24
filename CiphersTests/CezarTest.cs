using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class CezarTest
    {
        [TestMethod]
        public void ReturnsEncryptedWord()
        {
            string input = "CRYPTOGRAPHY";
            string expected = "FUBSWRJUDSKB";

            Cezar cezar = new Cezar(3);

            string encrypted = cezar.Encrypt(input);
            Assert.AreEqual(expected, encrypted);
        }

        [TestMethod]
        public void ReturnsDecryptedWord()
        {
            string input = "FUBSWRJUDSKB";
            string expected = "CRYPTOGRAPHY";

            Cezar cezar = new Cezar(3);

            string encrypted = cezar.Decrypt(input);
            Assert.AreEqual(expected, encrypted);
        }

    }
}