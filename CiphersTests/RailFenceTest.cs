using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class RailFenceTest
    {
        [TestMethod]
        public void ReturnsUnchangedWordFor1Key()
        {
            int key = 1;
            string input = "Dsadajkaajddjh";

            RailFence railFence = new RailFence(1);

            string encrypted = railFence.Encrypt(input);
            Assert.AreEqual(input, encrypted);
        }
    }
}