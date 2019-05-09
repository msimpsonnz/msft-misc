using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Shared
{
    public class ScoreValidator
    {
        public static int GetOnlineProfile(string url)
        {
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            return ExtractScore(htmlDoc);
        }

        public  static int ExtractScore(HtmlDocument htmlDoc)
        {
            var scoreString = htmlDoc.DocumentNode.SelectNodes("//div[@class='card']//p")[2].InnerText;
            return ScoreSplitter(scoreString);
        }

        public static int ExtractScore(RecognitionResult resultObj)
        {
            string exp = resultObj.recognitionResult.lines.Where(x => x.text.EndsWith(" XP")).Select(x => x.text).FirstOrDefault();
            return ScoreValidator.ScoreSplitter(exp);
        }

        public static int ScoreSplitter(string scoreString)
        {
            int score = 0;
            int.TryParse(scoreString.Split('/')[0], out score);
            return score;
        }

        public static string ExtractUrl(RecognitionResult resultObj)
        {
            return resultObj.recognitionResult.lines.Where(x => x.text.StartsWith("https://techprofile.microsoft.com")).Select(x => x.text).FirstOrDefault();

        }

    }
}
