using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFCiphers.Ciphers;

namespace CiphersTests
{
    [TestClass]
    public class DESTest
    {
        [TestMethod]
        public void BitsDoNotChangeBlocksAfterPermutation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(new string('0', 64));
            builder.Append(new string('1', 64));
            string input = builder.ToString();

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("PerformInitialPermutation", input));
            
            Assert.AreEqual(res, input);
        }

        [TestMethod]
        public void KeyBitsChangeAfterPermutation()
        {
            string expected = "11110110100010111011001001000111101011000101110101101100";
            DES des = new DES("0100010111101010100111010101001010101010101000101001111011101100");
            
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("PerformKeyPermutation"));
            
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void ReturnsUnchangedBitsAfterShiftByZero()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(new string('0', 14));
            builder.Append(new string('1', 14));
            string input = builder.ToString();

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("Shift", input, 0));
            
            Assert.AreEqual(res, input);
        }
        
        [TestMethod]
        public void ReturnsShiftedBits()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(new string('0', 14));
            builder.Append(new string('1', 14));
            string input = builder.ToString();
            builder.Clear();

            int shiftCount = 2;
            builder.Append(new string('0', 12));
            builder.Append(new string('1', 14));
            builder.Append(new string('0', 2));
            string expected = builder.ToString();

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("Shift", input, shiftCount));
            
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void CalculatesValueWithOnlyZeroes()
        {
            string input = "0000";
            int expected = 0;

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            int res = Convert.ToInt16(obj.Invoke("ConvertBitsToDecimal", input));
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void CalculatesValueWithOnlyOnes()
        {
            string input = "1111";
            int expected = 15;

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            int res = Convert.ToInt16(obj.Invoke("ConvertBitsToDecimal", input));
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void CalculatesValueWithBothOnesAndZeroes()
        {
            string input = "0101";
            int expected = 5;

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            int res = Convert.ToInt16(obj.Invoke("ConvertBitsToDecimal", input));
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void ReturnsUnchangedBinaryValueWhenItHas4Bits()
        {
            int input = 13;
            string expected = "1101";

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("ConvertDecimalToBits", input));
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void ReturnsAppendedBinaryValueWhenItDoesNotHave4Bits()
        {
            int input = 2;
            string expected = "0010";

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("ConvertDecimalToBits", input));
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void Returns4ZeroesForZeroValue()
        {
            int input = 0;
            string expected = "0000";

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("ConvertDecimalToBits", input));
            
            Assert.AreEqual(expected, res);
        }
        
        [TestMethod]
        public void ReturnsStartingDataAfterEncryptionAndDecryption()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";
            
            DES des = new DES("0100010111101010100111010101001010101010101000101001111011101100");
            PrivateObject obj = new PrivateObject(des);

            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            string encrypted = Convert.ToString(obj.Invoke("Encrypt", appended));

            string decryptedAppended = Convert.ToString(obj.Invoke("Decrypt", encrypted));
            string decrypted = Convert.ToString(obj.Invoke("RemoveAppendedBitsWithExtension", decryptedAppended));

            Assert.AreEqual(start, decrypted);
        }

        // weak key tests
        // here encryption and decryption gives identical results
        [TestMethod]
        public void EncryptionAndDecryptionReturnsSameResultWhenKeyContainsOnly0()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";
            
            DES des = new DES(new string('0', 64));
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string encrypted = Convert.ToString(obj.Invoke("Encrypt", appended));
            string decrypted = Convert.ToString(obj.Invoke("Decrypt", appended));

            Assert.AreEqual(encrypted, decrypted);
        }
        
        [TestMethod]
        public void EncryptionAndDecryptionReturnsSameResultWhenKeyContainsOnly1()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";
            
            DES des = new DES(new string('1', 64));
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string encrypted = Convert.ToString(obj.Invoke("Encrypt", appended));
            string decrypted = Convert.ToString(obj.Invoke("Decrypt", appended));

            Assert.AreEqual(encrypted, decrypted);
        }
        
        [TestMethod]
        public void EncryptionAndDecryptionReturnsSameResultWhenKeyContainsAnEdgeCaseKey()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";

            // in hex this key is E1E1E1E1F0F0F0F0
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 4; i++) builder.Append("11100001");
            for (int i = 0; i < 4; i++) builder.Append("11110000");

            DES des = new DES(builder.ToString());
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string encrypted = Convert.ToString(obj.Invoke("Encrypt", appended));
            string decrypted = Convert.ToString(obj.Invoke("Decrypt", appended));

            Assert.AreEqual(encrypted, decrypted);
        }
        
        [TestMethod]
        public void EncryptionAndDecryptionReturnsSameResultWhenKeyContainsAnEdgeCaseKeyV2()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";

            // in hex this key is 1E1E1E1E0F0F0F0F
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 4; i++) builder.Append("00011110");
            for (int i = 0; i < 4; i++) builder.Append("00001111");

            DES des = new DES(builder.ToString());
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string encrypted = Convert.ToString(obj.Invoke("Encrypt", appended));
            string decrypted = Convert.ToString(obj.Invoke("Decrypt", appended));

            Assert.AreEqual(encrypted, decrypted);
        }
        
        // semi-weak key tests
        // here after two encryptions with two different keys we get the starting value
        [TestMethod]
        public void TwoEncryptionsGivesInitialValueWithAnEdgeCaseKeys()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";

            // in hex this key is 011F011F010E010E
            string firstKey = "0000000100011111000000010001111100000001000011100000000100001110";
            // in hex this key is 1F011F010E010E01
            string secondKey = "0001111100000001000111110000000100001110000000010000111000000001";

            DES des = new DES(firstKey);
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string firstEncryption = Convert.ToString(obj.Invoke("Encrypt", appended));

            des.Key = secondKey;
            string secondEncryption = Convert.ToString(obj.Invoke("Encrypt", firstEncryption));
            string res = Convert.ToString(obj.Invoke("RemoveAppendedBitsWithExtension", secondEncryption));

            Assert.AreEqual(start, res);
        }
        
        [TestMethod]
        public void TwoEncryptionsGivesInitialValueWithAnEdgeCaseKeysV2()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";

            // in hex this key is 1E001E001F101F1
            string firstKey = "0000000111100000000000011110000000000001111100010000000111110001";
            // in hex this key is E001E001F101F101
            string secondKey = "1110000000000001111000000000000111110001000000011111000100000001";

            DES des = new DES(firstKey);
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string firstEncryption = Convert.ToString(obj.Invoke("Encrypt", appended));

            des.Key = secondKey;
            string secondEncryption = Convert.ToString(obj.Invoke("Encrypt", firstEncryption));
            string res = Convert.ToString(obj.Invoke("RemoveAppendedBitsWithExtension", secondEncryption));

            Assert.AreEqual(start, res);
        }
        
        [TestMethod]
        public void TwoEncryptionsGivesInitialValueWithAnEdgeCaseKeysV3()
        {
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";

            // in hex this key is FE01FE01FE01FE
            string firstKey = "0000000011111110000000011111111000000001111111100000000111111110";
            // in hex this key is FE01FE01FE01FE01
            string secondKey = "1111111000000001111111100000000111111110000000011111111000000001";

            DES des = new DES(firstKey);
            PrivateObject obj = new PrivateObject(des);
            
            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));
            
            string firstEncryption = Convert.ToString(obj.Invoke("Encrypt", appended));

            des.Key = secondKey;
            string secondEncryption = Convert.ToString(obj.Invoke("Encrypt", firstEncryption));
            string res = Convert.ToString(obj.Invoke("RemoveAppendedBitsWithExtension", secondEncryption));

            Assert.AreEqual(start, res);
        }

        [TestMethod]
        public void AppendBitsWithExtensionTest()
        {
            string key = "0100010111101010100111010101001010101010101000101001111011101100";
            string start = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";
            string expected = "011101011010100011000111010100100010101010101011101101100010100001010010101010101010000000111010101010011000000000000000000000000000000000000000000000000000000000101110011101000111100001110100";

            DES des = new DES(key);
            PrivateObject obj = new PrivateObject(des);

            string extensionInBin = Convert.ToString(obj.Invoke("ConvertTextToBinaryString", ".txt"));
            string appended = Convert.ToString(obj.Invoke("AppendBitsWithExtension", start, extensionInBin));

            Assert.AreEqual(expected, appended);
        }

        [TestMethod]
        public void RemoveAppendedBitsWithExtensionTest()
        {
            string key = "0100010111101010100111010101001010101010101000101001111011101100";
            string start = "011101011010100011000111010100100010101010101011101101100010100001010010101010101010000000111010101010011000000000000000000000000000000000000000000000000000000000101110011101000111100001110100";
            string expected = "01110101101010001100011101010010001010101010101110110110001010000101001010101010101000000011101010101001";

            DES des = new DES(key);
            PrivateObject obj = new PrivateObject(des);

            string shorted = Convert.ToString(obj.Invoke("RemoveAppendedBitsWithExtension", start));

            Assert.AreEqual(expected, shorted);
        }
    }
}