using Azure.Data.Tables;
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

        }
        [TestMethod]
        public void ConfirmTenantIDAndOtherVariables()
        {
            Assert.AreEqual("75202359-8ca2-4185-af85-e8d288e60729", _tenantID);
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

        [TestCleanup]
        public void Cleanup()
        {
            //code to delete a table: _tableName
            //_tableServiceClient.DeleteTable(_tableName);
        }

        // install storage explorer from here: https://go.microsoft.com/fwlink/?linkid=2216182&clcid=0x409
    }
}
