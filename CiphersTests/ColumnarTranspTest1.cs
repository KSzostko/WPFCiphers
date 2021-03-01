using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class ColumnarTranspTest1
    {
        [TestMethod]
        public void EncryptTest()
        {
            int[] key = new int[] { 3, 1, 4, 2 };
            string input = "CRYPTOGRAPHYOSA";
            string expected = "YCPRGTROHAYPAOS";

            ColumnarTransposition ct = new ColumnarTransposition(key);
            string output = ct.Encrypt(input);

            Assert.AreEqual(output, expected);

        }

        [TestMethod]
        public void DecryptTest()
        {
            int[] key = new int[] { 3, 1, 4, 2 };
            string input = "YCPRGTROHAYPAOS"; 
            string expected = "CRYPTOGRAPHYOSA";

            ColumnarTransposition ct = new ColumnarTransposition(key);
            string output = ct.Decrypt(input);

            Assert.AreEqual(output, expected);

        }
    }
}
