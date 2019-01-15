using System;

namespace NoSQL.ConsoleApp
{
    public class DeviceModel
    {
        public DeviceModel()
        {
            Id = Guid.NewGuid().ToString();
            deviceid = HashHelper.GetPartitionKey(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            uid = Id;
            type = "record";
        }

        public string Id { get; set; }

        public string deviceid { get; set; }

        public string uid { get; set; }

        public string type { get; set; }
    }
}
