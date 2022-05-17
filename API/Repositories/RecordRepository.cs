using API.Models;
using System.Data.SqlClient;
using System.Data;
using API.Interfaces;
using API.Middlewares;
using API.DTOs;

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

        public int Insert(RecordDTO record)
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

        public IEnumerable<RecordDTO> GetRecordsByPageNo(string bookStatus, string email, int pageSize, int pageNumber)
        {
            List<RecordDTO> records = new();
            SqlConnection cnn = null;
            SqlDataReader reader = null;
            try
            {
                using (cnn = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("GetRecordsByPageNo", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RowsOfPage", pageSize);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        if (cnn.State == ConnectionState.Closed)
                        {
                            cnn.Open();
                        }
                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {

                            while (reader.Read())
                            {
                                records.Add(new RecordDTO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    BookId = Convert.ToInt32(reader["BookId"]),
                                    BookName = reader["Name"].ToString(),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Email = reader["Email"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    IssuedDate = DateTime.Parse(reader["IssuedDate"].ToString()),
                                    DueDate = DateTime.Parse(reader["DueDate"].ToString()),
                                    ReturnDate = DateTime.Parse(reader["ReturnDate"].ToString()),
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
        public IEnumerable<RecordDTO> GetStatus(int UserId)
        {
            List<RecordDTO> records = new();
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
                                records.Add(new RecordDTO()
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
        public void UpdateRecordById(int? id, RecordDTO record)
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
                        cmd.Parameters.AddWithValue("@IssuedDate", record.IssuedDate == null ? DBNull.Value : record.IssuedDate);
                        cmd.Parameters.AddWithValue("@DueDate", record.DueDate == null ? DBNull.Value : record.DueDate);
                        cmd.Parameters.AddWithValue("@ReturnDate", record.ReturnDate == null ? DBNull.Value : record.ReturnDate);
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
