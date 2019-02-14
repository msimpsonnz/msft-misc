using System.Linq;

namespace Portal.Shared
{

    public class RecognitionResult
    {
        public string status { get; set; }
        public Recognitionresult recognitionResult { get; set; }

        public string GetXP(RecognitionResult resultObj)
        {
            return resultObj.recognitionResult.lines.Where(x => x.text.EndsWith(" XP")).Select(x => x.text).FirstOrDefault();
        }
    }

    public class Recognitionresult
    {
        public Line[] lines { get; set; }
    }

    public class Line
    {
        public int[] boundingBox { get; set; }
        public string text { get; set; }
        public Word[] words { get; set; }
    }

    public class Word
    {
        public int[] boundingBox { get; set; }
        public string text { get; set; }
        public string confidence { get; set; }
    }

}
