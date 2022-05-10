using API.Models;

namespace API.Interfaces
{
  public interface IBookRepository
  {
      int Insert(Book book);
      IEnumerable<Book> GetBooks();
      IEnumerable<Book> GetBooksByPageNo(int number);
      IEnumerable<Book> Search(string? book_name);
      int UpdateBookById(int? id, Book book);
      //void DeleteBook(int? id);
      public Book GetBookById(int id);
      void UpdateQuantity(int bookId, int amount);
  }
}