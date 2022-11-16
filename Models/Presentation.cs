namespace RestApiProject.Models
{
    public class Presentation
    {
        public int PresentationId { get; set; }
        public int BookStore { get; set; }
        public string PresenationURL { get; set; } = string.Empty;
        public string VideoURL { get; set; } = string.Empty;
        public string BookReview { get; set; } = string.Empty;

        // this is one to many 
       // public Book Book { get; set; }
        public int BookId { get; set; } // this is a foreign key and will create a not null foreign key in this table
        public int SpeakerId { get; set; }
        public Book Book { get; set; }
        public Speaker Speaker { get; set; }
    }
}
