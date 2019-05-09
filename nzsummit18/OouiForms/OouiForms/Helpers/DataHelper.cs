using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using OouiForms.Models;
using System.Collections.ObjectModel;

namespace OouiForms.Helpers
{

    public class DataHelper
    {
        private static HttpClient httpClient = new HttpClient();

        internal static ObservableCollection<Product> GetData()
        {
            var request = httpClient.GetStringAsync("http://nzsummitdemoweb.azurewebsites.net/api/shop?api-version=2.0");
            var result = JsonConvert.DeserializeObject<ObservableCollection<Product>>(request.Result);
            return result;
        }
    }
}
