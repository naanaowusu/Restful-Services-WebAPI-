namespace RestApiProject.Models
{
    public class ClubMeeting
    {
        public int ClubMeetingId { get; set; }
        public DateTime MeetingDate { get; set; }
        //public Presentation Presentation { get; set; }
        public int PresentationId { get; set; }
    }
}
