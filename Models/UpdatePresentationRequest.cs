namespace RestApiProject.Models
{
    public class UpdatePresentationRequest
    {
        public int BookStore { get; set; }
        public string PresenationURL { get; set; } = string.Empty;
        public string VideoURL { get; set; } = string.Empty;
        public string BookReview { get; set; } = string.Empty;
    }
}
