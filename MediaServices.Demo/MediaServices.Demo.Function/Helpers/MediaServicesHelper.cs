using MediaServices.Demo.Function.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function.Helpers
{
    public class MediaServicesHelper
    {

        public static HttpRequestMessage SetupRequest(string bearer, HttpMethod method, string uri)
        {
            var msg = new HttpRequestMessage(method, uri);
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            msg.Headers.Add("DataServiceVersion", "3.0");
            msg.Headers.Add("MaxDataServiceVersion", "3.0");
            msg.Headers.Add("x-ms-version", "2.15");
            return msg;
        }

        public static async Task<ReservedUnits> GetCurrentReservedUnits(HttpClient httpClient, string accessToken)
        {
            HttpMethod method = HttpMethod.Get;
            string uri = "https://mjsdemoams.restv2.centralus.media.azure.net/api/EncodingReservedUnitTypes";
            var msg = SetupRequest(accessToken, method, uri);
            var request = await httpClient.SendAsync(msg);
            request.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<ReservedUnits>(await request.Content.ReadAsStringAsync());
            return result;

        }

        public static async Task IncreaseReservedUnits(HttpClient httpClient, string accessToken, string accountId, int reservedUnits)
        {
            HttpMethod method = HttpMethod.Put;
            string uri = $"https://mjsdemoams.restv2.centralus.media.azure.net/api/EncodingReservedUnitTypes(guid'{accountId}')";
            var msg = SetupRequest(accessToken, method, uri);
            var body = $"{{ \"CurrentReservedUnits\": {reservedUnits} }}";
            msg.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var request = await httpClient.SendAsync(msg);
            request.EnsureSuccessStatusCode();

        }

        public static async Task<ReservedUnits> ScaleUpReservedUnits(HttpClient httpClient, string accessToken)
        {
            var currentUnits = await GetCurrentReservedUnits(httpClient, accessToken);

            if (currentUnits.value[0].CurrentReservedUnits <= 4)
            {
                int newUnitValue = currentUnits.value[0].CurrentReservedUnits + 1;
                await IncreaseReservedUnits(httpClient, accessToken, currentUnits.value[0].AccountId ,newUnitValue);
                var newCurrentUnits = await GetCurrentReservedUnits(httpClient, accessToken);
                return newCurrentUnits;
            }
            else
            {
                return currentUnits;
            }
        }

        public static async Task<ReservedUnits> ScaleDownReservedUnits(HttpClient httpClient, string accessToken)
        {
            var currentUnits = await GetCurrentReservedUnits(httpClient, accessToken);

            if (currentUnits.value[0].CurrentReservedUnits > 0)
            {
                int newUnitValue = currentUnits.value[0].CurrentReservedUnits - 1;
                await IncreaseReservedUnits(httpClient, accessToken, currentUnits.value[0].AccountId, newUnitValue);
                var newCurrentUnits = await GetCurrentReservedUnits(httpClient, accessToken);
                return newCurrentUnits;
            }
            else
            {
                return currentUnits;
            }
        }


    }
}
