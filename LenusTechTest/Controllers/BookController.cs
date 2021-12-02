using System;
using System.Linq;
using LenusTechTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace LenusTechTest.Controllers
{
    [Route("books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreContext _context;
        private long _idCount;

        public BookController(BookStoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            try
            {
                string validBook = ValidateBook(book);
                if (string.IsNullOrEmpty(validBook))
                {
                    book.Id = GetNextId();
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest(validBook);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            try
            {
                return Ok(_context.Books
                    .Select(x => new Book(x.Id, x.Author, x.Title, x.Price))
                    .OrderBy(x => x.Title)
                    .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult UpdateBook([FromRoute] long id, [FromBody] Book book)
        {
            try
            {
                string validBook = ValidateBook(book);
                if(string.IsNullOrEmpty(validBook))
                {
                    if (BookExists(id))
                    {
                        GetBookWithId(id).UpdateBook(book);
                        _context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Book with id " + id + " does not exist.");
                    }
                }
                else
                {
                    return BadRequest(validBook);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetBookFromId([FromRoute] long id)
        {
            try
            {
                if(BookExists(id))
                    return Ok(GetBookWithId(id));
                else
                    return BadRequest("Book with id " + id + " does not exist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult DeleteBookFromId([FromRoute] long id)
        {
            try
            {
                if (BookExists(id))
                {
                    _context.Remove(GetBookWithId(id));
                    _context.SaveChanges();
                    return Ok();
                }
                else
                    return BadRequest("Book with id " + id + " does not exist.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Book GetBookWithId(long id)
        {
            return _context.Books.Where(x => x.Id == id).FirstOrDefault();
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(x => x.Id == id);
        }

        private string ValidateBook(Book book)
        {
            if (string.IsNullOrEmpty(book.Author))
                return "Author is required";
            else if (string.IsNullOrEmpty(book.Title))
                return "Title is required";
            else
                return null;
        }

        private long GetNextId()
        {
            return _context.Books.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
        }
    }
}
