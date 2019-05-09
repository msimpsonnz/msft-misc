namespace NoSQL.Common.Helpers
{
    public class DocumentHelper
    {
        public static string GenerateRandomDocumentString(string id, string partitionKeyProperty, object parititonKeyValue)
        {
            return "{\n" +
                "    \"id\": \"" + id + "\",\n" +
                "    \"uid\": \"" + id + "\",\n" +
                "    \"" + partitionKeyProperty + "\": \"" + parititonKeyValue + "\",\n" +
                "    \"type\": \"event\"" +
                "}";
        }
    }
}
