using API.Models;
using System.Data.SqlClient;
using System.Data;
using API.Interfaces;
using API.Middlewares;

namespace API.Repositories
{
    public class BookRepository : IBookRepository
    {
        public string connectionString { get; set; }
        public IConfiguration _configuration;

        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public int Insert(Book book)
        {
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("InsertBook", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", book.Name);
                        cmd.Parameters.AddWithValue("@TotalQuantity", book.TotalQuantity);
                        cmd.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);
                        cmd.Parameters.AddWithValue("@Author", book.Author);
                        cmd.Parameters.AddWithValue("@Description", book.Description);
                        cmd.Parameters.AddWithValue("@CoverImage", book.CoverImage);
                        cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
                        cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                        cmd.Parameters.AddWithValue("@PublicationDate", book.PublicationDate);
                        cmd.Parameters.AddWithValue("@Pages", book.Pages);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        Int32 newId = (Int32)cmd.ExecuteScalar();
                        return newId;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cnn != null) cnn.Close();
            }
        }

        public Book GetBookById(int id)
        {
            Book book = new Book();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetBookById", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                book.Id = (int)reader["Id"];
                                book.Name = (string)reader["Name"];
                                book.TotalQuantity = (int)reader["TotalQuantity"];
                                book.AvailableQuantity = (int)reader["AvailableQuantity"];
                                book.Author = (string)reader["Author"];
                                book.Description = (string)reader["Description"];
                                book.CoverImage = (string)reader["CoverImage"];
                                book.Publisher = (string)reader["Publisher"];
                                book.ISBN = (int)reader["ISBN"];
                                book.PublicationDate = DateTime.Parse(reader["PublicationDate"].ToString());
                                book.Pages = (int)reader["Pages"];
                            }
                        }
                        return book;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (cnn != null) cnn.Close();
            }
        }

        public IEnumerable<Book> GetBooksByPageNo(int number)
        {
            List<Book> books = new List<Book>();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetBooksByPageNo", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", number);
                        cmd.Parameters.AddWithValue("@RowsOfPage", 14);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Book book = new Book();
                                book.Id = (int)reader["Id"];
                                book.Name = (string)reader["Name"];
                                book.TotalQuantity = (int)reader["TotalQuantity"];
                                book.AvailableQuantity = (int)reader["AvailableQuantity"];
                                book.Author = (string)reader["Author"];
                                book.Description = (string)reader["Description"];
                                book.CoverImage = (string)reader["CoverImage"];
                                book.Publisher = (string)reader["Publisher"];
                                book.ISBN = (int)reader["ISBN"];
                                book.PublicationDate = DateTime.Parse(reader["PublicationDate"].ToString());
                                book.Pages = (int)reader["Pages"];
                                book.MaxRows = (int)reader["MaxRows"];
                                books.Add(book);
                            }
                        }
                        return books;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (cnn != null) cnn.Close();
            }
        }

        public IEnumerable<Book> GetBooks()
        {
            List<Book> books = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetAllBooks", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                books.Add(new Book()
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["Name"].ToString(),
                                    TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                                    AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"]),
                                    Author = reader["Author"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CoverImage = reader["CoverImage"].ToString(),
                                    Publisher = reader["Publisher"].ToString(),
                                    ISBN = Convert.ToInt32(reader["ISBN"]),
                                    PublicationDate = DateTime.Parse(reader["PublicationDate"].ToString()),
                                    Pages = Convert.ToInt32(reader["Pages"])
                                });
                            }
                            return books;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (cnn != null) cnn.Close();
            }
        }
        public IEnumerable<Book> Search(string? bookName)
        {
            List<Book> books = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("SearchBooks", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookName", bookName);

                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }

                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                books.Add(new Book()
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["Name"].ToString(),
                                    TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                                    AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"]),
                                    Author = reader["Author"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CoverImage = reader["CoverImage"].ToString(),
                                    Publisher = reader["Publisher"].ToString(),
                                    ISBN = Convert.ToInt32(reader["ISBN"]),
                                    PublicationDate = DateTime.Parse(reader["PublicationDate"].ToString()),
                                    Pages = Convert.ToInt32(reader["Pages"])
                                });
                            }
                            return books;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (cnn != null) cnn.Close();
            }
        }

        public int UpdateBookById(int? id, Book book)
        {
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("UpdateBookById", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Name", book.Name);
                        cmd.Parameters.AddWithValue("@TotalQuantity", book.TotalQuantity);
                        cmd.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);
                        cmd.Parameters.AddWithValue("@Author", book.Author);
                        cmd.Parameters.AddWithValue("@Description", book.Description);
                        cmd.Parameters.AddWithValue("@CoverImage", book.CoverImage);
                        cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
                        cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                        cmd.Parameters.AddWithValue("@PublicationDate", book.PublicationDate);
                        cmd.Parameters.AddWithValue("@Pages", book.Pages);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        int RowsAffected = cmd.ExecuteNonQuery();
                        if (RowsAffected == 0)
                        {
                            throw new AppException("Failed to update book");
                        }
                        return RowsAffected;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cnn != null) cnn.Close();
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
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("ChangeAvailableQuantity", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookId", bookId);
                        cmd.Parameters.AddWithValue("@AMOUNT", amount);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cnn != null) cnn.Close();
            }
        }
    }
}
