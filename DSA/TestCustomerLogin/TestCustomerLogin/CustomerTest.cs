using CustomerLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCustomerLogin
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void TestCustomerConstructor()
        {
            Customer testCustomer = new Customer("Alice", "alice@live.com", "password", "11112222");
            Assert.AreEqual("Alice", testCustomer.Name);
            Assert.AreEqual("alice@live.com", testCustomer.Email);
            Assert.AreEqual(64, testCustomer.PasswordHash.Length);
            Assert.AreEqual(32, testCustomer.CreditCardHash.Length);
        }

    }
}

