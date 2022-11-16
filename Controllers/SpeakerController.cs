using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;
using RestApiProject.Models;

namespace RestApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpeakerController : ControllerBase
    {
        private readonly DataBaseContext dataBaseContext;

        public SpeakerController(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpeaker()
        {
            return Ok(await dataBaseContext.Speakers.ToListAsync());
        }

        [HttpGet]
        [Route("{SpeakerId}")]
        public async Task<IActionResult> GetSpeakerById(int SpeakerId) 
        {
            var getSpeaker = await dataBaseContext.Speakers.FindAsync(SpeakerId);
            if (getSpeaker == null)
            {
               return NotFound();
            }
            else
                return Ok(getSpeaker);
        }

        [HttpGet]
        [Route("keyword")]
        public IActionResult GetbyFilter(string keyword) 
        {
            
            var Speakerfilter =  dataBaseContext.Speakers.Where(f => f.FirstName.ToLower().Contains(keyword)||
            f.LastName.ToLower().Contains(keyword) ||
            f.SpeakerId.ToString().ToLower().Contains(keyword));

            return Ok(Speakerfilter);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpeaker(AddSpeakerRequest addSpeaker)
        {
            var speakers = new Speaker()
            {
                SpeakerId = new int(),
                FirstName = addSpeaker.FirstName,
                LastName = addSpeaker.LastName,
                
            };

            await dataBaseContext.Speakers.AddAsync(speakers);
            await dataBaseContext.SaveChangesAsync();

            return Ok(speakers);
        }

        [HttpPut]
        [Route("{SpeakerId}")]
        public async Task<IActionResult> UpdateSpeaker(int SpeakerId, UpdateSpeakerRequest updateSpeakerRequest) 
        {
            var findAndAddSpeaker = await dataBaseContext.Speakers.FindAsync(SpeakerId);
            if (findAndAddSpeaker != null)
            {
                findAndAddSpeaker.FirstName = updateSpeakerRequest.FirstName;
                findAndAddSpeaker.LastName = updateSpeakerRequest.LastName;
             
                await dataBaseContext.SaveChangesAsync();
                return Ok(findAndAddSpeaker);
            }
            else 
            {
                return NotFound("No such speakerId!");
            }
        }
        [HttpDelete]
        [Route("{SpeakerId}")]
        public async Task<IActionResult> DeleteSpeaker(int SpeakerId) 
        {
            var findAndDelete = await dataBaseContext.Speakers.FindAsync(SpeakerId);
            if (findAndDelete != null)
            {
                dataBaseContext.Speakers.Remove(findAndDelete);
                await dataBaseContext.SaveChangesAsync();

                return Ok("Speaker deleted!");
            }
            else 
            {
                return NotFound("Provide the correct SpeakerId!");
            }
        }

        
    }
}
