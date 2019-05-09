using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoSQL.Common.Helpers;
using NoSQL.Infrastructure;
using System;

namespace NoSQL.Tests
{
    [TestClass]
    public class DocumentTest
    {
        [TestMethod]
        public void GetDocumentBatch()
        {
            const string hashResult = "4FD33DCDE4707D09696A";
            var result = CosmosHelper.DocumentBatch("device", 100, 1);
            var test = "";

            //Assert.AreEqual(hashResult, hash);
            
        }

        [TestMethod]
        public void GetHashFails()
        {
            const string hashResult = "4FD33DCDE4707D09696B";

            Guid guid = new Guid("47d1fe45-667f-4a8d-9e16-a2caba598172");
            int bits = BitConverter.ToInt32(guid.ToByteArray(), 0);


            var hash = HashHelper.GetPartitionKey(bits);

            Assert.AreNotEqual(hashResult, hash);

        }
    }
}
