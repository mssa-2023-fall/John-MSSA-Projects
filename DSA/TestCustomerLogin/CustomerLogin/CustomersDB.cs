using Azure.Data.Tables;
using Azure.Security.KeyVault.Keys.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLogin
{
    public class CustomersDB
    {
        private Dictionary<string, Customer> _customers=new ();
        private CryptoHelper _cryptoHelper= new();
        public Customer this[string email] => _customers[email];
        public int Count => _customers.Count;
        public CustomersDB()
        {
            Customer alice = new Customer("Alice", "alice@live.com", "password", "11112222");
            Customer bob = new Customer("bob", "bob@live.com", "password", "11112222");
            Customer charlie = new Customer("charlie", "charlie@live.com", "password", "11112222");

            _customers.Add(alice.Email, alice);
            _customers.Add(bob.Email, bob);
            _customers.Add(charlie.Email, charlie);
        }

        public CustomersDB(TableClient _table, CryptographyClient _key)
        {
            Customer alice = new Customer("Alice", "alice@live.com", "password", "11112222", _key);
            Customer bob = new Customer("bob", "bob@live.com", "password", "11112222", _key);
            Customer charlie = new Customer("charlie", "charlie@live.com", "password", "11112222", _key);
            var AllCustomers = _table.Query<Customer>();
            foreach(var customer in AllCustomers)
            {
                _customers.Add(alice.Email, alice);
                _customers.Add(bob.Email, bob);
                _customers.Add(charlie.Email, charlie);
            }  
        }
        public bool Login(string username, string password) {
            if (!_customers.ContainsKey(username)) { return false; }
            else { 
                return _cryptoHelper.VerifyHash(password, _customers[username].PasswordHash, _customers[username].Salt);
            }
        }

        public string ReadCreditCard(string username, string password)
        {
            if (!_customers.ContainsKey(username)) { throw new Exception("Read Failed"); }
            var creditCardPlainText=string.Empty;
            try
            {
                creditCardPlainText = _cryptoHelper.DecryptCreditCard(password, _customers[username].CreditCardHash).Result;
            }
            catch (Exception)
            {

                throw new Exception("Read Failed"); ;
            }
                return creditCardPlainText;
        }

    }
}
