using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Order;

namespace Span.Demo
{
    [MemoryDiagnoser]
    public class StringBenchmarks
    {
        private class CustomConfig : ManualConfig
        {
            public CustomConfig()
            {
                Add(MemoryDiagnoser.Default);
            }
        }

        private const string FullName = "Matt Simpson";
        private static readonly StringHelper Parser = new StringHelper();

        [Benchmark]
        public void GetLastName()
        {
            Parser.GetLastName(FullName);
        }

        [Benchmark]
        public void GetLastNameUsingSpan()
        {
            Parser.GetLastNameWithSpan(FullName);
        }

    }
}