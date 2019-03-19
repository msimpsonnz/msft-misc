using System;
using Function.Demo.Span.Common;
using BenchmarkDotNet.Attributes;
using Microsoft.Azure.EventGrid.Models;
using System.IO;
using Newtonsoft.Json;

namespace Function.Demo.Span.Benchmark
{
    [MemoryDiagnoser]
    public class EventBenchmark
    {
        private static readonly FileHelper Helper = new FileHelper();
        private static string cwd = Directory.GetCurrentDirectory();
        private static EventGridEvent eventGridEvent;

        public EventBenchmark()
        {
            string sample_event = File.ReadAllText($"{cwd}\\event.json");
            eventGridEvent = JsonConvert.DeserializeObject<EventGridEvent>(sample_event);
        }

        [Benchmark]
        public void GetUsingSubstring()
        {
            Helper.CreateEventWithString(eventGridEvent);
        }

        [Benchmark]
        public void GetUsingSpan()
        {
            Helper.CreateEventWithSpan(eventGridEvent);
        }
    }
}