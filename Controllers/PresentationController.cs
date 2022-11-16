using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;

using RestApiProject.Models;

namespace RestApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PresentationController : ControllerBase
    {
        private readonly DataBaseContext dataBaseContext;

        public PresentationController(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPresentations()
        {
            //this is read all presenation
            return Ok(await dataBaseContext.Presentations.ToListAsync());
        }

        [HttpGet]
        [Route("{PresentationId}")]
        public async Task<IActionResult> FindPresntationById(int PresentationId) 
        {
            var findPresentation = await dataBaseContext.Presentations.FindAsync(PresentationId);
            if (findPresentation == null) 
            {
                return NotFound("Input the correct Id.");
            }
            else
                return Ok(findPresentation);
        }
        /// <summary>
        /// Implementation for using Parent and child relations
        /// The logic is to implement: presentation?expand=Book
        /// </summary>
        /// <param name="expand"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("expand")]
       
        public async Task<IActionResult> GetAllPresentationsExpand(string expand)
        {
            if (expand == "book")
            {
                return Ok(await dataBaseContext.Presentations.Include(p => p.Book).ToListAsync());
            }
            else if (expand == "speaker") 
            {
                return Ok(await dataBaseContext.Presentations.Include(p => p.Speaker).ToListAsync());
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddPresentation(AddPresentationRequest addPresentationRequest)
        {
            var presentations = new Presentation()
            {
                PresenationURL = addPresentationRequest.PresenationURL,
                BookStore = addPresentationRequest.BookStore,
                BookReview = addPresentationRequest.BookReview,
                VideoURL = addPresentationRequest.VideoURL,
                BookId = addPresentationRequest.BookId,
                SpeakerId = addPresentationRequest.SpeakerId
            };
            await dataBaseContext.AddAsync(presentations);
            await dataBaseContext.SaveChangesAsync();

            return Ok(presentations);
        }

        [HttpPut]
        [Route("{PresentationId}")]
        public async Task<IActionResult> UpdatePresentation(int PresentationId, UpdatePresentationRequest updatePresentation) 
        {
            var findAndUpdate = await dataBaseContext.Presentations.FindAsync(PresentationId);
            if (findAndUpdate != null)
            {
                findAndUpdate.BookStore = updatePresentation.BookStore;
                findAndUpdate.PresenationURL = updatePresentation.PresenationURL;
                findAndUpdate.BookReview = updatePresentation.BookReview;
                findAndUpdate.VideoURL = updatePresentation.VideoURL;

                await dataBaseContext.SaveChangesAsync();
                return Ok(findAndUpdate);
            }
            else 
            {
                return NotFound("There is no such id");
            }
        }

        [HttpDelete]
        [Route("PresentationId")]
        public async Task<IActionResult> DeletePresentation( int PresentationId) 
        {
            var findAndDelete = await dataBaseContext.Presentations.FindAsync(PresentationId);
            if (findAndDelete != null)
            {
                dataBaseContext.Presentations.Remove(findAndDelete);
                return Ok("Presntation deleted!");
            }
            else
                return NotFound("Specify the coreect Id");

        }

        [HttpGet]
        [Route("keyword")]
        public IActionResult GetByFilter(string keyword) 
        {
            var Presentationfilter = dataBaseContext.Presentations.Where(x => x.BookReview.ToLower().Contains(keyword) ||
            x.VideoURL.ToLower().Contains(keyword) ||
            x.BookStore.ToString().ToLower().Contains(keyword));

            return Ok(Presentationfilter);
        }
    }
}
