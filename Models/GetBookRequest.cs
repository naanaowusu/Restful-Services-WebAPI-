namespace RestApiProject.Models
{
    public class GetBookRequest
    {
      
        public int BookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

       

    }
}
