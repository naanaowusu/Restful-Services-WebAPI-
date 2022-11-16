using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;

using RestApiProject.Filter;
using RestApiProject.Models;
using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;


namespace RestApiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        /// <summary>
        /// Here we have to inject the DatabaseContext class so we can talk tro our database.
        /// Therefore we will create a costructor for the controller class
        /// </summary>
        /// <returns></returns>

        private readonly DataBaseContext dbContext;

        public BookController(DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await dbContext.Books.ToListAsync());
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var validBookId = await dbContext.Books.FindAsync(id);

            if (validBookId == null)
                return NotFound();

            var response = await dbContext.Books.Where(x => x.BookId == id).ToListAsync();

            return Ok(response);


        }


        /// <summary>
        ///  FILTERING
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("keyword")]
        public IActionResult GetBookByFilter(string keyword)
        { 
            keyword = keyword.ToLower();
            var filteringBooks = dbContext.Books.Where(x => x.Name.ToLower().Contains(keyword) ||
              x.Author.ToLower().Contains(keyword) ||
              x.PagesCount.ToString().ToLower().Contains(keyword) ||
              x.Description.ToLower().Contains(keyword) ||
              x.Tags.ToLower().ToLower().Contains(keyword));

            return Ok(filteringBooks);

        }


        /// <summary>
        /// PAGINATION
        /// This is the implementation for pagination
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<ActionResult<List<Book>>> GetBooks(int page)
        {
            // we first check our db if book is available
            if (dbContext.Books == null)
                return NotFound("There is no books");


            //now we set the number of results to see on a page
            var pageNumber = 3f; //the number of items to see in each page
            var pageCount = Math.Ceiling(dbContext.Books.Count() / pageNumber);

            var books = await dbContext.Books
                .Skip((page - 1) * (int)pageNumber) //so here if user ask for page 2 we skip previous
                .Take((int)pageNumber)
                .ToListAsync();

            // repsonse of the pages
            var response = new BookPagination()
            {
                Books = books,
                CurentPage = page,
                Pages = (int)pageCount
            };

            return Ok(books);
        }

        /// <summary>
        /// OPERATORS IMPLEMENTATION
        /// </summary>
        /// <returns></returns>
       // [HttpGet]
       //[Route("{books}")]
       // public IActionResult GetBookWithOperator(string fieldWithOperator)
       // {
       //     var position = fieldWithOperator.IndexOf("_");
       //     if (position >= 0) 
       //     {
       //         var op = fieldWithOperator.Substring(position + 1);
       //         var fieldName = fieldWithOperator.Substring(0, position);
       //         if (op == "gte") 
       //         {

       //         }
       //    }
           

           
    

        //operators gte,lte
        //[HttpGet]
        //public IActionResult GetBookbyOperators(string PageCount, string range) 
        //{
        //    var getbookid = dbContext.Books.Include(x => x.BookId).ToList();
        //}

        /// <summary>
        ///  FULL TEXT SEARCH 
        /// This is to perform a full text search: so in our case we search by book name
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("q")]
        public async Task<IActionResult> SearchByText(string q)
        {
            if (dbContext.Books == null)
                return NotFound("Input the correct parameters");

           // var response = await dbContext.Books.Where(x => x.Name.StartsWith(Text)).ToListAsync();
           var response = await dbContext.Books.Where(x => x.Name.ToLower().Contains(q) || 
           x.Author.ToLower().Contains(q) ||
           x.Tags.ToLower().Contains(q)).ToListAsync();
            
            return Ok(response);


        }


        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookRequest addBookRequest)
        {
            // create a new book object
            var book = new Book()
            {
                BookId = new int(),
                Name = addBookRequest.Name,
                Author = addBookRequest.Author,
                Description = addBookRequest.Description,
                PagesCount = addBookRequest.PagesCount,
                Tags = addBookRequest.Tags,
                CoverURL = addBookRequest.CoverURL
            };
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            return Ok(book);

        }
        
        [HttpPut]
        [Route("{BookId}")]
        public async Task<IActionResult> UpdateBook(int BookId, UpdateBookRequest updateBookRequest) 
        {
            //first find the id
            var findbook = await dbContext.Books.FindAsync(BookId);

            //if the bookid is not null the we update it else we retun not found
            if (findbook != null)
            {
                findbook.Author = updateBookRequest.Author;
                findbook.Name = updateBookRequest.Name;
                findbook.Description = updateBookRequest.Description;
                findbook.PagesCount = updateBookRequest.PagesCount;
                findbook.Tags = updateBookRequest.Tags;
                findbook.CoverURL = updateBookRequest.CoverURL;

                await dbContext.SaveChangesAsync();
                return Ok(findbook);
            }
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("{BookId}")]
        public async Task<IActionResult> DeleteBook( int BookId) 
        {
            var findToDelete = await dbContext.Books.FindAsync(BookId);

            if (findToDelete != null)
            {
                dbContext.Remove(findToDelete);
                await dbContext.SaveChangesAsync();

                return Ok(findToDelete);
            }
            else 
            {
                return NotFound("Provide the correct BookId!");
            }
        }

        

     
    }
}
