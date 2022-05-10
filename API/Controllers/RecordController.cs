using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private IRecordRepository _repository;
        private ILogger<RecordController> _logger;

        public RecordController(IRecordRepository repository, ILogger<RecordController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET Record
        [HttpGet, Authorize(Roles = "Admin")]
        public IEnumerable<Record> Get()
        {
            return _repository.GetRecords();
        }

        [HttpGet("page/{pageSize}/{currentPage}"), Authorize(Roles = "Admin,Student")]
        public IEnumerable<Record> GetBooksByPageNo(int pageSize, int currentPage)
        {
            return _repository.GetRecordsByPageNo(pageSize, currentPage);
        }

        [HttpGet("{id}"), Authorize(Roles = "Student")]
        public IEnumerable<Record> Get(int id)
        {
            return _repository.GetRecordsById(id);
        }

        [HttpGet("book/{UserId}"), Authorize(Roles = "Student")]
        public IEnumerable<Record> GetStatus(int UserId)
        {
            return _repository.GetStatus(UserId);
        }

        // POST Record
        [HttpPost, Authorize(Roles = "Student")]
        public int Post(Record record)
        {
            return _repository.Insert(record);
        }

        // PUT Record/5
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public void Put(int id, Record record)
        {
            
            _repository.UpdateRecordById(id, record);
        }

        // DELETE Record/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public void Delete(int id)
        {
            _repository.DeleteRecord(id);
        }
    }
}
