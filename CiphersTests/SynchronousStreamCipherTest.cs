using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class SynchronousStreamCipherTest
    {
        [TestMethod]
        public void EncryptTest()
        {
            List<bool> key = new List<bool> { true, false, false, false, true, false, true, true, true };
            string input = "test_ssc_encrypt.txt";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            BitArray output = ssc.Encrypt(input);

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void DecryptTest()
        {
            List<bool> key = new List<bool> { true, false, false, false, true, false, true, true, true };
            string input = "test_ssc_encrypt.txt";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            BitArray output = ssc.Decrypt(input);

            Assert.IsNotNull(output);
        }
    }
}
