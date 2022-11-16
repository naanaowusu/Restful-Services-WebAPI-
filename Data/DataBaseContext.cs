using Microsoft.EntityFrameworkCore;
using RestApiProject.Models;

namespace RestApiProject.Data
{
    public class DataBaseContext:DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<ClubMeeting> clubMeetings { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Speaker> Speakers { get; set; }

        
    }
}
