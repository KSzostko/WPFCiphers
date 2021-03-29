using System;
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
        public void ReturnsInputAppendedWith1WhenRemainderOfDivisionBy64Is63()
        {
            string input = "010101011110101000010101010100101010101010101010101101101010100";
            // appended only by 1
            string expected = "0101010111101010000101010101001010101010101010101011011010101001";

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("AppendBits", input));
            
            Assert.AreEqual(res, expected);
        }
        
        [TestMethod]
        public void ReturnsInputAppendedWith1AndZeroesWhenRemainderOfDivisionBy64IsLessThan63()
        {
            string input = "0101010111101010000101010101001010101010101010101011011010101";
            // appended by 1 and two zeros
            string expected = "0101010111101010000101010101001010101010101010101011011010101100";

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("AppendBits", input));
            
            Assert.AreEqual(res, expected);
        }
        
        [TestMethod]
        public void ReturnsUnchangedInputWhenItCanBeDividedBy64()
        {
            string input = "0101010111101010000101010101001010101010101010101011011010101000";

            DES des = new DES("0000010111101010000101010101001010101010101010101011011010101100");
            PrivateObject obj = new PrivateObject(des);
            string res = Convert.ToString(obj.Invoke("AppendBits", input));
            
            Assert.AreEqual(res, input);
        }
        
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
    }
}