using Amazon.DynamoDBv2.DataModel;

namespace NoSQL.ConsoleApp
{
    [DynamoDBTable("DeviceInventory")]
    public class DynamoDeviceModel : DeviceModel
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBRangeKey]
        public string deviceid { get; set; }

    }
}
