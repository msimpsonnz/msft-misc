using System;
using BenchmarkDotNet.Running;

namespace MediaServices.Demo.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FileBenchmarks>();
        }
    }
}
