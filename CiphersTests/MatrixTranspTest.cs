using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class MatrixTranspTest
    {
        [TestMethod]
        public void ReturnsUnchangedWordFor1LetterKeyEncryption()
        {
            string input = "Dsadajkaajddjh";

            MatrixTransp tp = new MatrixTransp("a");

            string encrypted = tp.Encrypt(input);
            Assert.AreEqual(input, encrypted);
        }
        
        [TestMethod]
        public void ReturnsUnchangedWordFor1LetterKeyDecryption()
        {
            string input = "Dsadajkaajddjh";

            MatrixTransp tp = new MatrixTransp("a");

            string decrypted = tp.Decrypt(input);
            Assert.AreEqual(input, decrypted);
        }
    }
}