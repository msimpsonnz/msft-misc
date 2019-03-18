using System;

namespace Span.Demo
{
    public class StringHelper
    {
        public string GetLastName(string fullName)
        {
            return fullName.Substring(fullName.LastIndexOf(' ') + 1);
        }



        public ReadOnlySpan<char> GetLastNameWithSpan(ReadOnlySpan<char> fullName)
        {

            var lastSpaceIndex = fullName.LastIndexOf(' ');

            return lastSpaceIndex == -1
                ? ReadOnlySpan<char>.Empty
                : fullName.Slice(lastSpaceIndex + 1);
        }
    }
}