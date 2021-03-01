using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class RailFenceTest
    {
        [TestMethod]
        public void ReturnsUnchangedWordFor1KeyEncryption()
        {
            string input = "Dsadajkaajddjh";

            RailFence railFence = new RailFence(1);

            string encrypted = railFence.Encrypt(input);
            Assert.AreEqual(input, encrypted);
        }
        
        [TestMethod]
        public void ReturnsUnchangedWordFor1KeyDecryption()
        {
            string input = "Dsadajkaajddjh";

            RailFence railFence = new RailFence(1);

            string decrypted = railFence.Decrypt(input);
            Assert.AreEqual(input, decrypted);
        }
        
        [TestMethod]
        public void ReturnsUnchangedWordForVeryLongKeyEncryption()
        {
            string input = "Dsadajkaajddjh";

            RailFence railFence = new RailFence(100);

            string encrypted = railFence.Encrypt(input);
            Assert.AreEqual(input, encrypted);
        }
        
        [TestMethod]
        public void ReturnsUnchangedWordForVeryLongKeyDecryption()
        {
            string input = "Dsadajkaajddjh";

            RailFence railFence = new RailFence(100);

            string decrypted = railFence.Decrypt(input);
            Assert.AreEqual(input, decrypted);
        }

        [TestMethod]
        public void ReturnsEncryptedWordForKeyShorterThanWord()
        {
            string input = "CRYPTOGRAPHY";
            string expected = "CTARPORPYYGH";

            RailFence railFence = new RailFence(3);

            string encrypted = railFence.Encrypt(input);
            Assert.AreEqual(encrypted, expected);
        }
        
        [TestMethod]
        public void ReturnsDecryptedWordForKeyShorterThanWord()
        {
            string input = "CTARPORPYYGH";
            string expected = "CRYPTOGRAPHY";

            RailFence railFence = new RailFence(3);

            string decrypted = railFence.Decrypt(input);
            Assert.AreEqual(decrypted, expected);
        }
    }
}