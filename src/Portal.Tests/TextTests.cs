using Newtonsoft.Json;
using Portal.Shared;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Portal.Tests
{
    public class TextTests
    {
        private static string cwd = Directory.GetCurrentDirectory();
        [Fact]
        public void ExtractXPFromTextResultSuccess()
        {
            const string expectedXP = "700/1799 XP";
            string sample_data = File.ReadAllText($"{cwd}\\Samples\\textResult.json");

            RecognitionResult recognitionResultData = JsonConvert.DeserializeObject<RecognitionResult>(sample_data);
            RecognitionResult recognitionResult = new RecognitionResult();

            string result = recognitionResult.GetXP(recognitionResultData);


            Assert.Equal(expectedXP, result);
        }
    }
}
