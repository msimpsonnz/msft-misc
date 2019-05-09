using System;
using System.Globalization;
using System.IO;
using System.Net.Http;

namespace Portal.Shared
{
    public class BlobUploader
    {
        public static HttpRequestMessage BlobUploadRequest(MemoryStream memoryStream, string uri)
        {
            byte[] bytes = memoryStream.ToArray();

            int contentLength = bytes.Length;
            string now = DateTime.Now.ToString("R", CultureInfo.InvariantCulture);

            var request = new HttpRequestMessage(HttpMethod.Put, uri);

            request.Content = new ByteArrayContent(bytes);
            request.Content.Headers.ContentLength = contentLength;

            request.Headers.Add("x-ms-version", "2018-03-28");
            request.Headers.Add("x-ms-date", now);
            request.Headers.Add("x-ms-blob-type", "BlockBlob");

            return request;
        }

    }
}
