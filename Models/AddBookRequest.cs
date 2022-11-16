namespace RestApiProject.Models
{
    public class AddBookRequest
    {
        /// <summary>
        /// So this addbook request allow user to add a book with his own id
        /// </summary>
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PagesCount { get; set; }
        public string CoverURL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
    }
}
