using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;
using RestApiProject.Models;

namespace RestApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClubMeetingController : ControllerBase
    {
        private readonly DataBaseContext dataBaseContext;

        public ClubMeetingController(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadMeeting()
        {
            return Ok(await dataBaseContext.clubMeetings.ToListAsync());
        }

        [HttpGet]
        [Route("{ClubMeetingId}")]
        public async Task<IActionResult> ReadMeetingById(int ClubMeetingId)
        {
            var findById = await dataBaseContext.clubMeetings.FindAsync(ClubMeetingId);
            if (findById == null)
            {
                return NotFound("There is no such meeting.");
            }
            else 
            {
                return Ok(findById);
            }
        }

       

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(AddClubMeeting addClubMeeting) 
        {
            var clubMeeting = new ClubMeeting()
            {
                MeetingDate = DateTime.UtcNow,
                PresentationId = addClubMeeting.PresentationId,

            };
            await dataBaseContext.clubMeetings.AddAsync(clubMeeting);
            await dataBaseContext.SaveChangesAsync();

            return Ok(clubMeeting);
        }

        [HttpPut]
        [Route("{ClubMeetingId}")]
        public async Task<IActionResult> UpdateMeeting(int ClubMeetingId, UpdateClubMeeting updateClub) 
        {
           var updateMeet =  await dataBaseContext.clubMeetings.FindAsync(ClubMeetingId);
            if (updateMeet != null)
            {
                updateMeet.MeetingDate = DateTime.UtcNow;
                updateMeet.PresentationId = updateClub.PresentationId;

                await dataBaseContext.clubMeetings.AddAsync(updateMeet);
                await dataBaseContext.SaveChangesAsync();

                return Ok(updateMeet);
            }
            else
                return NotFound("Meeing id not found");
        }

        [HttpDelete]
        [Route("{ClubMeetingId}")]
        public async Task<IActionResult> DeleteMeeting(int ClubMeetingId) 
        {
            var findAndDeleteMeeting = await dataBaseContext.clubMeetings.FindAsync(ClubMeetingId);
            if (findAndDeleteMeeting != null)
            {
                dataBaseContext.clubMeetings.Remove(findAndDeleteMeeting);
                return Ok("Meeting delected");
            }
            else
                return NotFound("Enter the correct meeting Id");
        }
    }
}
