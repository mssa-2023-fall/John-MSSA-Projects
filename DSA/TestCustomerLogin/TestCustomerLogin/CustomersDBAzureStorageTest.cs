//#define DeleteTables

using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using CustomerLogin;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestCustomerLogin
{
    [TestClass]
    public class CustomersDBAzureStorageTest
    {
        private string _tenantID;
        private string _clientID;
        private string _ClientSecret;
        private string _tableConnStr;
        private string _tableName;
        private string _keyVaultUri;
        private TableServiceClient _tableServiceClient;
        private KeyClient _keyClient;
        private CryptographyClient _key;

        [TestInitialize]
        public void init()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();
            _tenantID = config["Azure:TenantID"];
            _clientID = config["Azure:ClientID"];
            _ClientSecret = config["Azure:ClientSecret"];
            _tableConnStr = config["Azure:TableConnStr"];
            _tableName = config["Azure:TableName"] + Guid.NewGuid().ToString().Substring(0, 4);//append guid to avoid delay in deleting the table
            _keyVaultUri = config["Azure:KeyVaultUri"];
            //code to create a table: _tableName
            _tableServiceClient = new TableServiceClient(_tableConnStr);
            _tableServiceClient.CreateTableIfNotExists(_tableName);
    #if DeleteTables
            foreach(var t in _tableServiceClient(_tableName))
            {
                _tableServiceClient.DeleteTableAsync(t.Name);
            }
            Task.Delay(10000);
    #endif

            //create key client
            _keyClient = new KeyClient(new Uri(_keyVaultUri), new ClientSecretCredential(_tenantID, _clientID, _ClientSecret));
            _key = _keyClient.GetCryptographyClient("mssa");

        }

        [TestMethod]
        public void EncDecWithKeyVault()
        {
            var plainText = "HelloWorld";
            byte[] encryptedResult = _key.Encrypt(EncryptionAlgorithm.RsaOaep, Encoding.UTF8.GetBytes(plainText)).Ciphertext;
            byte[] decryptedBytes = _key.Decrypt(EncryptionAlgorithm.RsaOaep, encryptedResult).Plaintext;
            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            Assert.AreEqual(plainText, decryptedText);
        }



        [TestMethod]
        public void ConfirmTenantIDAndOtherVariables()
        {
            Assert.AreEqual("75202359-8ca2-4185-af85-e8d288e60729", _tenantID);
            Assert.AreEqual("595c2c90-3888-428a-9b28-4574aeb9b706", _clientID);
        }
        [TestMethod]
        public void EntityInsertAndRetreiveTest()
        {
            TableClient _testTable = _tableServiceClient.GetTableClient(_tableName);
            var tableEntity = new TableEntity("x", "y")
                {
                    { "Product", "Marker Set" },
                    { "Price", 5.00 },
                    { "Quantity", 21 }
                };
            _testTable.AddEntity(tableEntity);
            var result = _testTable.GetEntity<TableEntity>("x", "y").Value;

            Assert.AreEqual("Marker Set", result.GetString("Product"));
            Assert.AreEqual(5.00d, result.GetDouble("Price"));
            Assert.AreEqual(21, result.GetInt32("Quantity"));
        }
        //Insert a test record and confirm we can read it back
        [TestMethod]
        public void CustomerInsertAndRetreiveTest()
        {
            TableClient _testTable = _tableServiceClient.GetTableClient(_tableName);
            Customer testCustomer = new Customer("alice", "alice@live.com", "password", "11112222");
            _testTable.AddEntity(testCustomer);
            Customer result = _testTable.GetEntity<Customer>("alice@live.com", "alice");

            Assert.AreEqual(result.Name, testCustomer.Name);
            Assert.AreEqual(result.Email, testCustomer.Email);
            Assert.AreEqual(Convert.ToBase64String(result.PasswordHash), Convert.ToBase64String(testCustomer.PasswordHash));
            Assert.AreEqual(Convert.ToBase64String(result.CreditCardHash), Convert.ToBase64String(testCustomer.CreditCardHash));
        }


        public void CustomerInsertAndLinqQueryTest()
        {
            TableClient _testTable = _tableServiceClient.GetTableClient(_tableName);
            Customer testCustomer1 = new Customer("bob", "bob@live.com", "password", "11112222");
            Customer testCustomer2 = new Customer("bob", "bob@gmail.com", "password", "11112222");
            Customer testCustomer3 = new Customer("bob", "bob@outlook.com", "password", "11112222");
            _testTable.AddEntity(testCustomer1);
            _testTable.AddEntity(testCustomer2);
            _testTable.AddEntity(testCustomer3);

            var result = _testTable.Query<Customer>(c => c.RowKey == "bob");

            Assert.AreEqual(1, result.Where(c => c.Email == "bob@gmail.com").Count());
            Assert.AreEqual(2, result.Where(c => c.Email == "bob@outlook.com").Count());
            Assert.AreEqual(3, result.Count());

        }
        public void CustomerInsertAndFilterQueryTest()
        {
            TableClient _testTable = _tableServiceClient.GetTableClient(_tableName);
            Customer testCustomer1 = new Customer("bob", "bob@live.com", "password", "11112222");
            Customer testCustomer2 = new Customer("bob", "bob@gmail.com", "password", "11112222");
            Customer testCustomer3 = new Customer("bob", "bob@outlook.com", "password", "11112222");
            _testTable.AddEntity(testCustomer1);
            _testTable.AddEntity(testCustomer2);
            _testTable.AddEntity(testCustomer3);

            var result = _testTable.Query<Customer>(filter: TableClient.CreateQueryFilter($"Name eq 'bob'"), maxPerPage:2);

            Assert.AreEqual(1, result.Where(c => c.Email == "bob@gmail.com").Count());
            Assert.AreEqual(2, result.Where(c => c.Email == "bob@outlook.com").Count());
            Assert.AreEqual(2, result.AsPages().Count());
            
        }

        [TestMethod]
        public void CustomerWithCryptoClient()
        {
            TableClient _testTable = _tableServiceClient.GetTableClient(_tableName);
            Customer testCustomer = new Customer("bob", "bob@live.com", "password", "11112222", _key);
            _testTable.AddEntity(testCustomer);

            Customer result = _testTable.GetEntity<Customer>("alice@live.com", "alice");

            Assert.AreEqual(result.Name, testCustomer.Name);
            Assert.AreEqual(result.Email, testCustomer.Email);
            Assert.AreEqual(Convert.ToBase64String(result.CreditCardHash), Convert.ToBase64String(testCustomer.CreditCardHash));
            Assert.AreEqual(256, result.CreditCardHash.Length);
            string clearTextCreditcardEncoding = Encoding.UTF8.GetString(_key.Decrypt(EncryptionAlgorithm.RsaOaep, result.CreditCardHash).Plaintext);
            Assert.AreEqual("11112222", clearTextCreditcardEncoding);

        }

        [TestCleanup]
        public void Cleanup()
        {
            //code to delete a table: _tableName
            //_tableServiceClient.DeleteTable(_tableName);
        }

        // install storage explorer from here: https://go.microsoft.com/fwlink/?linkid=2216182&clcid=0x409
    }
}

