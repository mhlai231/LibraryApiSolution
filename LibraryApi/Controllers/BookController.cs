using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class BookController : ControllerBase
    {
        LibraryDataContext _context;

        IMapper _mapper;
        MapperConfiguration _config;

        public BookController(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        // GET  /books
        [HttpGet("books")]
        public async Task<ActionResult> GetAllBooks([FromQuery]string genre = "All")
        {
            var response = new GetBooksResponse();
            var results = _context.Books
                .Where(b => b.RemovedFromInventory == false);

            if (genre != "All")
            {
                results = results.Where(b => b.Genre == genre);
            }
            response.Data = await results.ProjectTo<GetBooksResponseItem>(_config).ToListAsync();

            response.Genre = genre;
            response.Count = response.Data.Count;


            return Ok(response); //TODO THIS SUCKS FIX THIS!

        }

        [HttpGet("books/{bookId:int}")]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            var response = await _context.Books
                .Where(b => b.RemovedFromInventory == false && b.Id == bookId)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);

            }
         }

        [HttpDelete("books/{bookId:int}")]
        public async Task<ActionResult> RemoveBookFromInventory(int bookId)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId && b.RemovedFromInventory == false);

            if (book != null)
            {
                book.RemovedFromInventory = true;
                await _context.SaveChangesAsync();
            }

            return NoContent(); // HTTP "Fine"
        }
    }
}
