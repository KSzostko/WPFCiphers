using System;
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
            string input = "SECRET";
            string expected = "110110001000000010100001101000110011110111101000";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            string output = ssc.Encrypt(input);

            Assert.AreEqual(output, expected);
        }

        [TestMethod]
        public void DecryptTest()
        {
            List<bool> key = new List<bool> { true, false, false, false, true, false, true, true, true };
            string input = "110110001000000010100001101000110011110111101000";
            string expected = "SECRET";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            string output = ssc.Decrypt(input);

            Assert.AreEqual(output, expected);
        }
    }
}

/*
 using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    class SynchronousStreamCipherTest
    {
        [TestMethod]
        public void EncryptTest()
        {
            List<bool> key = new List<bool> { true, false, false, false, true, false, true, true, true };
            string input = "SECRET";
            string expected = "110110001000000010100001101000110011110111101000";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            string output = ssc.Encrypt(input);

            Assert.AreEqual(output, expected);
        }

        [TestMethod]
        public void DecryptTest()
        {
            List<bool> key = new List<bool> { true, false, false, false, true, false, true, true, true };
            string input = "";
            string expected = "HEREISASECRETMESSAGEENCIPHEREDBYTRANSPOSITION";

            SynchronousStreamCipher ssc = new SynchronousStreamCipher(key);
            string output = ssc.Decrypt(input);

            Assert.AreEqual(output, expected);
        }
    }
}

*/