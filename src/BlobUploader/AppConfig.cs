using System;

namespace BlobUploader
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }
        public string StorageContainer { get; set; }
        public string LocalDir { get; set; }
    }

}