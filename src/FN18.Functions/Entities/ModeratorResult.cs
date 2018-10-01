namespace FN18.Functions
{
    public class ModeratorResult
    { 
        public string OriginalText { get; set; }
        public string NormalizedText { get; set; }
        public string AutoCorrectedText { get; set; }
        public object[] Misrepresentation { get; set; }
        public Classification Classification { get; set; }
        public Status Status { get; set; }
        public PII PII { get; set; }
        public string Language { get; set; }
        public Terms[] Terms { get; set; }
        public string TrackingId { get; set; }
    }

    public class Classification
    {
        public Category1 Category1 { get; set; }
        public Category2 Category2 { get; set; }
        public Category3 Category3 { get; set; }
        public bool ReviewRecommended { get; set; }
    }

    public class Category1
    {
        public float Score { get; set; }
    }

    public class Category2
    {
        public float Score { get; set; }
    }

    public class Category3
    {
        public float Score { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public object Exception { get; set; }
    }

    public class PII
    {
        public object[] Email { get; set; }
        public object[] SSN { get; set; }
        public object[] IPA { get; set; }
        public object[] Phone { get; set; }
        public object[] Address { get; set; }
    }

    public class Terms
    {
        public int Index { get; set; }
        public int OriginalIndex { get; set; }
        public int ListId { get; set; }
        public string Term { get; set; }
    }

}
