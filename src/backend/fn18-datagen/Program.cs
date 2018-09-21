using System;

using Bogus;

namespace fn18_datagen
{
    class Program
    {
        static void Main(string[] args)
        {
            var fakes = Fake();

        }


        static void Fake()
        {
            var faker = new Faker("en");
            var o = new Order()
            {
                OrderId = faker.Random.Number(1, 100),
                Item = faker.Lorem.Sentence(),
                Quantity = faker.Random.Number(1, 10)
            };
            o.Dump();
        }


    }
}
