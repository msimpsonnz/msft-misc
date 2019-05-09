using System;

namespace BlobUploader
{
    public class AppConfig
    {
        public string[] StorageUrl { get; set; }
        public string StorageAccount { get; set; }
        public string StorageKey { get; set; }
        public string StorageContainer { get; set; }
        public string Sas { get; set; }
        public string File { get; set; }
    }

}