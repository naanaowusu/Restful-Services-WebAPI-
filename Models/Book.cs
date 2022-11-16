namespace RestApiProject.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PagesCount { get; set; }
        public string CoverURL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        //public int PresentationId { get; set; }

        //public ICollection<Presentation> Presentations { get; set; }
    }
}
