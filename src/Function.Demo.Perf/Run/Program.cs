using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bogus;

namespace Run
{
    class Program
    {
        private static string cwd = Directory.GetCurrentDirectory();

        static async Task Main(string[] args)
        {
            // string collectionJson = File.ReadAllText($"{cwd}\\postman_collection.json");
            // dynamic collection = JsonConvert.DeserializeObject<dynamic>(collectionJson);
            string fakeUsersJson;
            List<User> users = new List<User>();

            try
            {
                fakeUsersJson = File.ReadAllText($"{cwd}\\fakeusers.json");
                users = JsonConvert.DeserializeObject<List<User>>(fakeUsersJson);
            }
            catch (System.Exception)
            {
                users = GetSampleUsers(10, true);
            }

            //List<HttpRequestMessage> registerUsers = new List<HttpRequestMessage>();

            //string regUrl = "https://mjsdemofuncauth.azurewebsites.net/api/UserRegistration";

            //foreach (var p in users)
            //{
            //    var body = JsonConvert.SerializeObject(p);
            //    registerUsers.Add(HttpHelper.MakeRequest(regUrl, HttpMethod.Post, body));
            //}

            //await HttpHelper.SendRequest(registerUsers);

            List<UserRequest> tokenRequests = new List<UserRequest>();

            string tokenUrl = "https://mjsdemofuncauth.azurewebsites.net/api/user/";

            foreach (var u in users)
            {
                UserRequest tokenRequest = new UserRequest();
                tokenRequest.User = u;
                var body = JsonConvert.SerializeObject(u);
                tokenRequest.TokenReq = HttpHelper.MakeRequest(tokenUrl+u.Id, HttpMethod.Post, body);
                tokenRequests.Add(tokenRequest);
            }

            string dataUrl = "https://mjsdemofuncauth.azurewebsites.net/api/data/";

                
            foreach (var req in tokenRequests)
            {
                var body = JsonConvert.SerializeObject(req.User);
                await HttpHelper.AuthRequest(req.TokenReq, dataUrl, req.User, body);
            }


        }

        public class UserRequest
        {
            public User User { get; set; }

            public HttpRequestMessage TokenReq { get; set; }

            public HttpRequestMessage DataReq { get; set; }
        }

        public static List<User> GetSampleUsers(int numberOfFakes, bool save = false)
        {
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Guid().ToString())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.EmailAddress, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Password, f => f.Internet.Password());

            var users = userFaker.Generate(numberOfFakes);

            if (save)
            {
                var output = JsonConvert.SerializeObject(users, Formatting.Indented);
                var path = $"{cwd}\\fakeusers.json";
                using (var file = new StreamWriter(path))
                {
                    file.Write(output);
                    file.Close();
                    file.Dispose();
                }
            }

            return users;
        }

    }
}
