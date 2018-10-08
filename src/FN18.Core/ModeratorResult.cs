namespace FN18.Core
{
    public class ModeratorResult
    {
        public string Id { get; set; }
        public string OriginalText { get; set; }
        public string EmailDetected { get; set; }
        public string EmailText { get; set; }
        public string Term { get; set; }
        public bool Flagged { get; set; }
    }
}
