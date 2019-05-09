using System;
using System.Security.Cryptography;
using System.Text;

namespace NoSQL.Common.Helpers
{
    public class HashHelper
    {
        public static string GetPartitionKey(int partitionKey)
        {
            SHA256 sha256 = SHA256.Create();
            int bucket = partitionKey % 64;
            var partition = sha256.ComputeHash(BitConverter.GetBytes(bucket));
            var hash = GetStringFromHash(partition);
            return hash;

        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}
