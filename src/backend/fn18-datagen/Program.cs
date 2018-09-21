using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bogus;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace fn18_datagen
{
    class Program
    {
        public static IConfiguration config { get; set; }

        private DocumentClient client;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json");
            config = builder.Build();

            List<User> users = Fake(10);
            Program p = new Program();
            p.CreateDoc(config, users).Wait();

        }


        static List<User> Fake(int numUsers)
        {
            Randomizer.Seed = new Random(3897234);

            var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

            var orderIds = 0;
            var testOrders = new Faker<Order>()
               //Ensure all properties have rules. By default, StrictMode is false
               //Set a global policy by using Faker.DefaultStrictMode if you prefer.
               .StrictMode(true)
               //OrderId is deterministic
               .RuleFor(o => o.OrderId, f => orderIds++)
               //Pick some fruit from a basket
               .RuleFor(o => o.Item, f => f.PickRandom(fruit))
               //A random quantity from 1 to 10
               .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));


            var userIds = 0;
            var testUsers = new Faker<User>()
               //Optional: Call for objects that have complex initialization
               .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))

                //Basic rules using built-in generators
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")
                .RuleFor(u => u.SomeGuid, Guid.NewGuid)

                //Use an enum outside scope.
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                //Use a method outside scope.
                .RuleFor(u => u.CartId, f => Guid.NewGuid())
                //Compound property with context, use the first/last name properties
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                //And composability of a complex collection.
                .RuleFor(u => u.Orders, f => testOrders.Generate(3))
                //After all rules are applied finish with the following action
                .FinishWith((f, u) => { Console.WriteLine("User Created! Name={0}", u.FullName); });

            List<User> result = new List<User>();
            result.AddRange(testUsers.Generate(numUsers));
            Console.WriteLine("Done");

            return result;

        }

        public async Task CreateDoc(IConfiguration config, List<User> users)
        {

            this.client = new DocumentClient(new Uri(config["EndpointUri"]), config["PrimaryKey"]);
            foreach (User u in users)
            {
                await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(config["Database"], config["Collection"]), u);
                System.Console.WriteLine($"Created {u.FullName}");
            }

        }

        public class User
        {
            public User(int userId, string ssn)
            {
                this.UserId = userId;
                this.SSN = ssn;
            }

            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string SomethingUnique { get; set; }
            public Guid SomeGuid { get; set; }

            public string Avatar { get; set; }
            public Guid CartId { get; set; }
            public string SSN { get; set; }
            public Gender Gender { get; set; }

            public List<Order> Orders { get; set; }
        }

        public enum Gender
        {
            Male,
            Female
        }


        public class Order
        {
            public int OrderId { get; set; }
            public string Item { get; set; }
            public int Quantity { get; set; }
        }

        public class OrderFaker : Faker<Order>
        {
            public OrderFaker() : base("en")
            {
                RuleFor(o => o.OrderId, f => f.Random.Number(1, 100));
                RuleFor(o => o.Item, f => f.Lorem.Sentence());
                RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));
            }
        }


    }
}
