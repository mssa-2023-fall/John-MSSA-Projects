using CustomerLogin;
using System.Security.Cryptography;

namespace TestCustomerLogin
{
    [TestClass]
    public class CryptoHelperTests
    {

        CryptoHelper _helper = new CryptoHelper();
        const int keySize= 64;
        byte[] salt = Enumerable.Repeat((byte)1, keySize).ToArray();
        string expectedString = "B4CC6561E8F1AE876184A069933E139B08C9429BFB1729EFAAE7DC9EBC172990199546B0218B00998806041D5253DDE52CC803ECB088AF674A25A60BBA3222B1";
        string creditCardString = "11112222";
        string expectedBase64EncryptedCreditCard = "IwFP8Z3gLFjjbhadZx75VvmUWRNttoud09KKxG6Jwc4=";
        [TestMethod]
        public void TestHashing()
        {
            string password = "password";

            var hashedString = Convert.ToHexString(_helper.ComputeHash(password, salt));
            Assert.AreEqual(expectedString, hashedString);

        }
        [TestMethod]
        public void VerifyHash()
        {
            string password = "password";

            var result = _helper.VerifyHash(password, Convert.FromHexString(expectedString),salt);

            Assert.IsTrue(result);

        }
        [TestMethod]
        public void VerifyWrongHash()
        {
            string password = "NotCorrectPassword";

            var result = _helper.VerifyHash(password, Convert.FromHexString(expectedString), salt);

            Assert.IsFalse(result);

        }

        [TestMethod]
        public void EncryptCreditCardFromPlainText()
        {
            string password = "password";
            var result = Convert.ToBase64String(_helper.EncryptCreditCard(password, creditCardString).Result);
            Assert.AreEqual(expectedBase64EncryptedCreditCard,result);           
        }
        [TestMethod]
        public void DecryptCreditCardFromCipher()
        {
            string password = "password";
            byte[] encryptedCreditCard = Convert.FromBase64String(expectedBase64EncryptedCreditCard);
            var result = _helper.DecryptCreditCard(password, encryptedCreditCard).Result;
            Assert.AreEqual(creditCardString, result);
        }

        [TestMethod]
        public void DecryptCreditCardFromCipherWithWrongPassword()
        {
            string password = "NotCorrectPassword";
            byte[] encryptedCreditCard = Convert.FromBase64String(expectedBase64EncryptedCreditCard);
            Assert.ThrowsException<AggregateException>(() => _helper.DecryptCreditCard(password, encryptedCreditCard).Result);
            
        }
    }
}