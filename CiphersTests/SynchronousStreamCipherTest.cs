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
            string input = "input.txt";
            string expected = "1010010101100011001011001101111101010110110010000010011001011011";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            PrivateObject obj = new PrivateObject(ssc);

            BitArray output = ssc.Encrypt(input);
            string output_string = Convert.ToString(obj.Invoke("BitArrayToString", output));

            Assert.AreEqual(output_string, expected);
        }
        
        [TestMethod]
        public void DecryptTest()
        {
            List<bool> key = new List<bool> { true, false, false, false, true, false, true, true, true };
            string input = "encrypted_ssc.bin";
            string expected = "00101110101001101100111000101110";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            PrivateObject obj = new PrivateObject(ssc);

            BitArray output = ssc.Decrypt(input);
            string output_string = Convert.ToString(obj.Invoke("BitArrayToString", output));

            Assert.AreEqual(output_string, expected);
        }
        
    }
}
