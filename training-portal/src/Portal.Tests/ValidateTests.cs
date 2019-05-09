using HtmlAgilityPack;
using Portal.Shared;
using System.IO;
using Xunit;

namespace Portal.Tests
{
    public class ValidateTests
    {
        private static string cwd = Directory.GetCurrentDirectory();
        [Fact]
        public void ExtractXPFromTextResultSuccess()
        {
            const int expectedXP = 400;

            var doc = new HtmlDocument();
            doc.Load($"{cwd}\\Samples\\onlineprofile.html");

            var result = ScoreValidator.ExtractScore(doc);


            Assert.Equal(expectedXP, result);
        }


        [Fact]
        public void ExtractXPFromStringuccess()
        {
            const string XPstring = "700/1799 XP";
            const int expectedXP = 700;

            var result = ScoreValidator.ScoreSplitter(XPstring);


            Assert.Equal(expectedXP, result);
        }
    }
}
