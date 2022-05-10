using API.Models;
using System.Data.SqlClient;
using System.Data;
using API.Interfaces;
using API.Middlewares;

namespace API.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        public string connectionString { get; set; }
        public IConfiguration _configuration;
        public RecordRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public int Insert(Record record)
        {
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("InsertRecord", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BookId", record.BookId);
                        cmd.Parameters.AddWithValue("@UserId", record.UserId);
                        cmd.Parameters.AddWithValue("@Status", record.Status);
                        cmd.Parameters.AddWithValue("@IssuedDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@DueDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ReturnDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FineAmount", record.FineAmount);
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

        public IEnumerable<Record> GetRecordsByPageNo(int pageSize, int currentPage)
        {
            List<Record> records = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetRecordssByPageNo", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {

                            while (reader.Read())
                            {
                                records.Add(new Record()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    BookId = Convert.ToInt32(reader["BookId"]),
                                    BookName = reader["Name"].ToString(),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Email = reader["Email"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    IssuedDate = reader["IssuedDate"].ToString(),
                                    DueDate = reader["DueDate"].ToString(),
                                    ReturnDate = reader["ReturnDate"].ToString(),
                                    FineAmount = Convert.ToInt32(reader["FineAmount"]),
                                    MaxRows = (int)reader["MaxRows"]
                                });
                            }
                            return records;
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

        public IEnumerable<Record> GetRecords()
        {
            List<Record> records = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetAllRecords", cnn))
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
                                records.Add(new Record()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    BookId = Convert.ToInt32(reader["BookId"]),
                                    BookName = reader["Name"].ToString(),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Email = reader["Email"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    IssuedDate = reader["IssuedDate"].ToString(),
                                    DueDate = reader["DueDate"].ToString(),
                                    ReturnDate = reader["ReturnDate"].ToString(),
                                    FineAmount = Convert.ToInt32(reader["FineAmount"])
                                });
                            }
                            return records;
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

        public IEnumerable<Record> GetStatus(int UserId)
        {
            List<Record> records = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetBookStatus", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {

                            while (reader.Read())
                            {
                                records.Add(new Record()
                                {
                                    BookId = Convert.ToInt32(reader["BookId"]),
                                    Status = reader["Status"].ToString(),
                                });
                            }
                            return records;
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
        public IEnumerable<Record> GetRecordsById(int? id)
        {
            List<Record> records = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetRecordsById", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", id);

                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }

                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    BookId = Convert.ToInt32(reader["BookId"]),
                                    BookName = reader["Name"].ToString(),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Email = reader["Email"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    IssuedDate = reader["IssuedDate"].ToString(),
                                    DueDate = reader["DueDate"].ToString(),
                                    ReturnDate = reader["ReturnDate"].ToString(),
                                    FineAmount = Convert.ToInt32(reader["FineAmount"])
                                });
                            }
                            return records;
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

        public void UpdateRecordById(int? id, Record record)
        {
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("UpdateRecordById", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Status", record.Status);
                        cmd.Parameters.AddWithValue("@IssuedDate", String.IsNullOrEmpty(record.IssuedDate) ? DBNull.Value : record.IssuedDate);
                        cmd.Parameters.AddWithValue("@DueDate", String.IsNullOrEmpty(record.DueDate) ? DBNull.Value : record.DueDate);
                        cmd.Parameters.AddWithValue("@ReturnDate", String.IsNullOrEmpty(record.ReturnDate) ? DBNull.Value : record.ReturnDate);
                        cmd.Parameters.AddWithValue("@FineAmount", record.FineAmount);
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
        public void DeleteRecord(int? id)
        {
            SqlConnection cnn = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("DeleteRecordById", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
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
