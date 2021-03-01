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
        
        [TestMethod]
        public void ReturnsLettersSpiltWithSpacesForVeryLongKeyWithSameLettersEncryption()
        {
            string input = "kot ma ale";
            string expected = "k o t m a a l e";

            MatrixTransp tp = new MatrixTransp("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            string encrypted = tp.Encrypt(input);
            Assert.AreEqual(expected, encrypted);
        }
        
        [TestMethod]
        public void ReturnsLettersWithoutSpacesForVeryLongKeyWithSameLettersDecryption()
        {
            string input = "k o t m a a l e";
            string expected = "kotmaale";

            MatrixTransp tp = new MatrixTransp("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            string decrypted = tp.Decrypt(input);
            Assert.AreEqual(expected, decrypted);
        }
    }
}