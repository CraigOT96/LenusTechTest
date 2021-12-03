using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LenusTechTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace LenusTechTest.Controllers
{
    [Route("books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BookController(BookStoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddBook(string title, string author, double price)
        {
            try
            {
                Book book = new Book(GetNextId(), title, author, price);
                string validBook = ValidateBook(book);
                if (string.IsNullOrEmpty(validBook))
                {
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    return Created("Created",  new { id = book.Id });
                }
                else
                {
                    return BadRequest(new { errors = new[]
                        {
                            new string[] { validBook }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    errors = new[]
                    {
                        new string[] { ex.Message }
                    }
                });
            }
        }

        [HttpGet]
        public IActionResult GetBooks([Optional] string sortby)
        {
            try
            {
                List<Book> books = _context.Books
                    .Select(x => new Book(x.Id, x.Author, x.Title, x.Price))
                    .ToList();
                switch(sortby)
                {
                    case "title":
                        return Ok(books.OrderBy(x => x.Title));
                    case "author":
                        return Ok(books.OrderBy(x => x.Author));
                    case "price":
                        return Ok(books.OrderBy(x => x.Price));
                    default:
                        return Ok(books.OrderBy(x => x.Title));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    errors = new[]
                    {
                            new string[] { ex.Message }
                        }
                });
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult UpdateBook([FromRoute] long id, string title, string author, double price)
        {
            try
            {
                Book book = new Book(id, title, author, price);
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
                        return NotFound(new
                        {
                            errors = new[]
                            {
                                new string[] { "Book with id " + id + " does not exist." }
                            }
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        errors = new[]
                        {
                            new string[] { validBook }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    errors = new[]
                    {
                        new string[] { ex.Message }
                    }
                });
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
                    return NotFound(new
                    {
                        errors = new[]
                        {
                                new string[] { "Book with id " + id + " does not exist." }
                            }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    errors = new[]
                    {
                            new string[] { ex.Message }
                        }
                });
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
                    return NotFound(new
                    {
                        errors = new[]
                        {
                            new string[] { "Book with id " + id + " does not exist." }
                        }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    errors = new[]
                    {
                            new string[] { ex.Message }
                        }
                });
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
