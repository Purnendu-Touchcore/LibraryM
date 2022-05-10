using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using API.Middlewares;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookRepository _repository;

        public BookController(IBookRepository repository)
        {
            _repository = repository;
        }

        // GET Book
        [HttpGet, Authorize(Roles = "Admin,Student")]
        public IEnumerable<Book> Get()
        {
            return _repository.GetBooks();
        }

        [HttpGet("page/{number}"), Authorize(Roles = "Admin,Student")]
        public IEnumerable<Book> GetBooksByPageNo(int number)
        {
            return _repository.GetBooksByPageNo(number);
        }

        // GET Book/book_name
        [HttpGet("search/{book_name}"), Authorize(Roles = "Admin,Student")]
        public IEnumerable<Book> Get(string book_name)
        {
            return _repository.Search(book_name);
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Student")]
        public Book Get(int id)
        {
            return _repository.GetBookById(id);
        }

        // POST Book
        [HttpPost, Authorize(Roles = "Admin")]
        public int Post(Book book)
        {
            return _repository.Insert(book);
        }

        // PUT Book/5
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public ActionResult<Status> Put(int id, Book book)
        {
            _repository.UpdateBookById(id, book);
            return Ok(new Status(){ message = "Book with id " +id+ " updated successfully"});
        }

        // DELETE Book/5
        // [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        // public void Delete(int id)
        // {
        //     _repository.DeleteBook(id);
        // }

        //Update Books Available Quantity
        [HttpGet("{bookId}/{amount}"), Authorize(Roles = "Admin,Student")]
        public void UpdateQuantity(int bookId, int amount)
        {
            _repository.UpdateQuantity(bookId, amount);
        }
    }
}
