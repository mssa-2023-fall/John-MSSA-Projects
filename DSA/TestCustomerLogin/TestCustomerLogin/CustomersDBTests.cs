using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomerLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLogin.Tests
{
    [TestClass()]
    public class CustomersDBTests
    {
        private CustomersDB _customers = new();
        [TestMethod()]
        public void LoginSuccessTest()
        {
            Assert.IsTrue(_customers.Login("alice@live.com", "password"));
        }
        [TestMethod()]
        public void LoginFailsWithWrongPasswordTest()
        {
            Assert.IsFalse(_customers.Login("alice@live.com", "badpassword"));
        }
        [TestMethod()]
        public void LoginFailsWithWrongUserNameTest()
        {
            Assert.IsFalse(_customers.Login("noone@live.com", "password"));
        }
        [TestMethod()]
        public void TestExistingCustomersCountEquals3()
        {
            Assert.AreEqual(3, _customers.Count);
        }
        [TestMethod()]
        public void PasswordHashShouldBeDifferent()
        {
            Assert.AreNotEqual(_customers["alice@live.com"].PasswordHash, _customers["bob@live.com"].PasswordHash);
            Assert.AreNotEqual(_customers["alice@live.com"].PasswordHash, _customers["charlie@live.com"].PasswordHash);
        }
        [TestMethod()]
        public void TestCreditCardDecryption()
        {
            Assert.AreEqual("11112222", _customers.ReadCreditCard("alice@live.com", "password"));
        }
        [TestMethod()]
        public void TestCreditCardDecryptionFailsWithWrongPassword()
        {
            Assert.ThrowsException<Exception>(() => _customers.ReadCreditCard("alice@live.com", "badpassword"));
        }
        [TestMethod()]
        public void TestCreditCardDecryptionFailsWithNoUserFound()
        {
            Assert.ThrowsException<Exception>(() => _customers.ReadCreditCard("noone@live.com", "password"));
        }

    }
}