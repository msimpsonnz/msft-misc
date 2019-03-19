using System;
using BenchmarkDotNet.Running;

namespace Function.Demo.Span.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<EventBenchmark>();
        }
    }
}
