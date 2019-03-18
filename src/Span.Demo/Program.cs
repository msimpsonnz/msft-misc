using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Span.Demo
{
    [MemoryDiagnoser]
    class Program
    {
        private const string fullName = "Matt Simpson";
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StringBenchmarks>();
            // StringBenchmarks str = new StringBenchmarks();
            // str.GetLastName();
            // str.GetLastNameUsingSpan();
        }
    }
}
