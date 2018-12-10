using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace NoSQL.ConsoleApp
{
    public class DynamoHelper
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public static void DynamoBulkImport()
        {


            var table = Table.LoadTable(client, "AnimalsInventory");
            var item = new DynamoDeviceModel();


        }
    }
}

