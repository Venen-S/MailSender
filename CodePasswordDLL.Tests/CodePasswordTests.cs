using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CodePasswordDLL.Test
{
    [TestClass]
    public class CodePasswordTests
    {
        CodePassword cp;

        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine("Test Initialize");
            cp = new CodePassword();
        }
        [TestMethod]
        public void getCodPassword_abc_bcd()
        {
            // arrange
            string strIn = "abc";
            string strExpected = "bcd";
            // act
            string strActual = CodePassword.getCodPassword(strIn);
            //assert
            Assert.AreEqual(strExpected, strActual);
        }

        [TestMethod()]
        public void getCodPassword_empty_empty()
        {
            string strIn = "";
            string strExpected = "";
            // act
            string strActual = CodePassword.getCodPassword(strIn);
            //assert
            Assert.AreEqual(strExpected, strActual);
        }

        [TestMethod]
        public void getPassword_bcd_abc()
        {
            string strIn = "bcd";
            string strExpected = "abc";
            string strActual = CodePassword.getPassword(strIn);
            Assert.AreEqual(strExpected, strActual);
        }
    }
}

