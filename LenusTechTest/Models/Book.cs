using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LenusTechTest.Models
{
    public class Book
    {
        public Book(long id, string title, string author, double price)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Price = price;
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public void UpdateBook(Book book)
        {
            this.Title = book.Title;
            this.Author = book.Author;
            this.Price = book.Price;
        }
    }
}
