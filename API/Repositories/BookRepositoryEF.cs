using API.Models;
using System.Data.SqlClient;
using System.Data;
using API.Interfaces;
using API.Middlewares;
using PagedList;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BookRepositoryEF : IBookRepository
    {
        public DataContext _context;

        public BookRepositoryEF(DataContext context)
        {
            _context = context;
        }

        public int Insert(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return book.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Book GetBookById(int id)
        {
            try
            {
                return _context.Books.Find(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Book> GetBooksByPageNo(int number)
        {
            try
            {
                var books = _context.Books.Skip((number - 1) * 14).Take(14).ToList(); //take 14 books per page
                books[0].MaxRows = _context.Books.Count();
                return books;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Book> GetBooks()
        {
            try
            {
                return _context.Books.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public IEnumerable<Book> Search(string? bookName)
        {
            try
            {

                return from b in _context.Books
                       where b.Name.Contains(bookName)
                       select b;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateBookById(int? id, Book book)
        {
            try
            {
                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();
                return id ?? 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // public void DeleteBook(int? id)
        // {
        //   using (SqlConnection cnn = new SqlConnection(connectionString))
        //   {
        //     using (SqlCommand cmd = new SqlCommand("DeleteBookById", cnn))
        //     {
        //       cmd.CommandType = CommandType.StoredProcedure;
        //       cmd.Parameters.AddWithValue("@Id", id);
        //       if (cnn.State == ConnectionState.Closed)
        //       {
        //         cnn.Open();
        //       }

        //       cmd.ExecuteNonQuery();
        //       cnn.Close();
        //     }
        //   }
        // }

        public void UpdateQuantity(int bookId, int amount)
        {
            try
            {
                var result = _context.Books.SingleOrDefault(b => b.Id == bookId);
                if (result != null)
                {
                    result.TotalQuantity = result.TotalQuantity + amount;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
