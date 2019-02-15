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
            const int expectedXP = 700;
            string sample_data = File.ReadAllText($"{cwd}\\Samples\\textResult.json");

            RecognitionResult recognitionResultData = JsonConvert.DeserializeObject<RecognitionResult>(sample_data);

            int result = ScoreValidator.ExtractScore(recognitionResultData);


            Assert.Equal(expectedXP, result);
        }

        [Fact]
        public void ExtractUrlFromTextResultSuccess()
        {
            const string expectedXP = "https://techprofile.microsoft.com/en-us/msimpsonnz";
            string sample_data = File.ReadAllText($"{cwd}\\Samples\\textResult.json");

            RecognitionResult recognitionResultData = JsonConvert.DeserializeObject<RecognitionResult>(sample_data);

            string result = ScoreValidator.ExtractUrl(recognitionResultData);


            Assert.Equal(expectedXP, result);
        }
    }
}
