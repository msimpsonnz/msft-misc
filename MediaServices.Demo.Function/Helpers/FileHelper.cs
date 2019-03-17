using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function.Helpers
{
    public class FileHelper
    {
        public static string GetFullFileName(string blobUri)
        {
            string fileFull = blobUri.Substring(blobUri.LastIndexOf('/') + 1);
            return fileFull;
        }

        public static string GetFileNameWithoutExt(string fileFull)
        {
            string fileId = fileFull.Substring(0, fileFull.LastIndexOf('.'));
            return fileId;
        }
    }
}