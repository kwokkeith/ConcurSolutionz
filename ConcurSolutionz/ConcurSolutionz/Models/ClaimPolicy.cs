namespace ConcurSolutionz.Models
{
    //Object to deserialize the different possible claim policies
    public class ClaimPolicy
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string MatchValue { get; set; }
        public string ServiceVersion { get; set; }
        public string ShortCode {get; set;}
        public string Value { get; set; }
        public string Typename { get; set; }

    }
}
