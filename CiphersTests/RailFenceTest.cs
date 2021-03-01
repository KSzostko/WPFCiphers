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
    }
}