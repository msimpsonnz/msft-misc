using System;
using Microsoft.Azure.EventGrid.Models;

namespace Function.Demo.Span.Common
{
    public class FileHelper
    {
        public Event CreateEventWithString(EventGridEvent eventGridEvent)
        {
            var lastSpaceIndex = eventGridEvent.Subject.LastIndexOf('/');
            var blobName = eventGridEvent.Subject.Substring(lastSpaceIndex + 1);
            return new Event()
            {
                Id = Guid.NewGuid().ToString(),
                BlobName = blobName,
                EventData = eventGridEvent.Data
            };
        }

        public Event CreateEventWithSpan(EventGridEvent eventGridEvent)
        {
            ReadOnlySpan<char> subject = eventGridEvent.Subject;
            var lastSpaceIndex = subject.LastIndexOf('/');
            return new Event()
            {
                Id = Guid.NewGuid().ToString(),
                BlobName = subject.Slice(lastSpaceIndex + 1).ToString(),
                EventData = eventGridEvent.Data
            };
        }
    }
}
