using API.Models;
using System.Data.SqlClient;
using System.Data;
using API.Interfaces;
using API.Middlewares;
using PagedList;
using API.DTOs;

namespace API.Repositories
{
    public class RecordRepositoryEF : IRecordRepository
    {
        public string connectionString { get; set; }
        public IConfiguration _configuration;
        public ILogger<RecordRepositoryEF> _logger;
        public DataContext _context;
        public RecordRepositoryEF(IConfiguration configuration, ILogger<RecordRepositoryEF> logger, DataContext context)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        public int Insert(RecordDTO recordDTO)
        {
            Record record = new Record{
                Id = 0,
                BookId = recordDTO.BookId,
                UserId = recordDTO.UserId,
                Status = recordDTO.Status,
                FineAmount = recordDTO.FineAmount
            };
            try
            {
                _context.Records.Add(record);
                _context.SaveChanges();
                return record.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<RecordDTO> GetRecordsByPageNo(string bookStatus, string email, int pageSize, int currentPage)
        {
            List<RecordDTO> recordDTOlist = new();

            try
            {
                List<Record> records = _context.Records.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                int? userId = 0;

                if(!email.Equals("All")) {
                    userId = _context.Users.Where(u => u.Email == email).First()?.Id;
                    records =  _context.Records.Where(r => r.UserId == userId).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                if(!bookStatus.Equals("All")){
                    records =  _context.Records.Where(r => r.Status == bookStatus).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }

                if(!email.Equals("All") && !bookStatus.Equals("All")){
                    userId = _context.Users.Where(u => u.Email == email).First()?.Id;
                    records =  _context.Records.Where(r => r.UserId == userId && r.Status == bookStatus).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }

                
                foreach(var record in records)
                {
                    recordDTOlist.Add(new RecordDTO {
                        Id = record.Id,
                        BookId = record.BookId,
                        BookName = _context.Books.Find(record.BookId)?.Name,
                        Status = record.Status,
                        IssuedDate = record.IssuedDate,
                        DueDate = record.DueDate,
                        ReturnDate = record.ReturnDate,
                        FineAmount = record.FineAmount,
                        UserId = record.UserId,
                        Email = _context.Users.Find(record.UserId)?.Email,
                        MaxRows = _context.Records.Count()
                    });
                }
                if(!email.Equals("All")){   
                    recordDTOlist[0].MaxRows = _context.Records.Where(r => r.UserId == userId).Count();
                }
                return recordDTOlist;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public IEnumerable<RecordDTO> GetStatus(int UserId)
        {
            List<RecordDTO> records = new();
            try
            {
                var record = (
                    from r in _context.Records 
                    join b in _context.Books 
                    on r.BookId equals b.Id
                    where r.UserId == UserId && r.Status != "Returned"
                    select new {
                        BookId = r.BookId,
                        Status = r.Status,
                    });

                foreach(var r in record){
                    records.Add(new RecordDTO {
                        BookId = r.BookId,
                        Status = r.Status
                    });
                }
                return records;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateRecordById(int? id, RecordDTO record)
        {
            try
            {
                var recordToUpdate = _context.Records.Find(id);
                recordToUpdate.Status = record.Status;
                recordToUpdate.IssuedDate = record.IssuedDate;
                recordToUpdate.DueDate = record.DueDate;
                recordToUpdate.ReturnDate = record.ReturnDate;
                recordToUpdate.FineAmount = record.FineAmount;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteRecord(int? id)
        {
            try
            {
                var record = _context.Records.Find(id);
                _context.Records.Remove(record);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
