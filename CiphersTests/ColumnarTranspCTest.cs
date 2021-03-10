using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class ColumnarTranspCTest
    {
        [TestMethod]
        public void EncryptTest()
        {
            string key = "CONVENIENCE";
            string input = "HERE IS A SECRET MESSAGE ENCIPHERED BY TRANSPOSITION";
            string expected = "HEESPNI RR SSEES EIY A SCBT EMGEPN ANDI CT RTAHSO IEERO";

            ColumnarTranspositionC ct = new ColumnarTranspositionC(key);
            string output = ct.Encrypt(input);

            Assert.AreEqual(output, expected);
        }

        [TestMethod]
        public void DecryptTest()
        {
            string key = "CONVENIENCE";
            string input = "HEESPNI RR SSEES EIY A SCBT EMGEPN ANDI CT RTAHSO IEERO";
            string expected = "HEREISASECRETMESSAGEENCIPHEREDBYTRANSPOSITION";

            ColumnarTranspositionC ct = new ColumnarTranspositionC(key);
            string output = ct.Decrypt(input);

            Assert.AreEqual(output, expected);
        }
    }
}

/*
 [TestClass]
    class ColumnarTranspCTest
    {
        [TestMethod]
        public void EncryptTest()
        {
            string key = "CONVENIENCE";
            string input = "HERE IS A SECRET MESSAGE ENCIPHERED BY TRANSPOSITION";
            string expected = "HEESPNI RR SSEES EIY A SCBT EMGEPN ANDI CT RTAHSO IEERO";

            ColumnarTranspositionC ct = new ColumnarTranspositionC(key);
            string output = ct.Encrypt(input);

            Assert.AreEqual(output, expected);
        }

        [TestMethod]
        public void DecryptTest()
        {
            string key = "CONVENIENCE";
            string input = "HEESPNI RR SSEES EIY A SCBT EMGEPN ANDI CT RTAHSO IEERO";
            string expected = "HEREISASECRETMESSAGEENCIPHEREDBYTRANSPOSITION";

            ColumnarTranspositionC ct = new ColumnarTranspositionC(key);
            string output = ct.Decrypt(input);

            Assert.AreEqual(output, expected);
        }
    }
 */
