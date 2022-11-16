namespace RestApiProject.Models
{
    public class Speaker
    {
        public int SpeakerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
       // public ICollection<Presentation> Presentations { get; set; }
    }
}
