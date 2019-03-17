using System;
using BenchmarkDotNet.Attributes;
using MediaServices.Demo.Function.Helpers;


namespace MediaServices.Demo.Benchmark
{
    [MemoryDiagnoser]
    public class FileBenchmarks
    {
        private const string BlobUri = @"https:\\somelongurlwitha\folder\filename.ext";
        // private static readonly NameParser Parser = new NameParser();

        [Benchmark(Baseline = true)]
        public void GetFileName()
        {
           FileHelper.GetFullFileName(BlobUri);
        }
    }
}