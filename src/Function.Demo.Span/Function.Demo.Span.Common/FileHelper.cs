using System;
using Microsoft.Azure.EventGrid.Models;

namespace Function.Demo.Span.Common
{
    public class FileHelper
    {
        public static string GetBlobName(EventGridEvent eventGridEvent)
        {
            return eventGridEvent.Subject.Substring(eventGridEvent.Subject.LastIndexOf('/') + 1);
        }

        public static ReadOnlySpan<char> GetBlobNameWithSpan(EventGridEvent eventGridEvent)
        {
            ReadOnlySpan<char> blob = eventGridEvent.Subject;
            var lastSpaceIndex = blob.LastIndexOf('/');

            return lastSpaceIndex == -1
                ? ReadOnlySpan<char>.Empty
                : blob.Slice(lastSpaceIndex + 1);
        }

    }
}
