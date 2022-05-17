using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;

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
        [HttpGet("{bookStatus}/{email}/{pageSize}/{pageNumber}")]
        public IEnumerable<RecordDTO> GetRecordsByPageNo(string bookStatus, string email, int pageSize, int pageNumber)
        {
            return _repository.GetRecordsByPageNo(bookStatus, email, pageSize, pageNumber);
        }


        // GET Book status
        [HttpGet("book/{UserId}"), Authorize(Roles = "Student")]
        public IEnumerable<RecordDTO> GetStatus(int UserId)
        {
            return _repository.GetStatus(UserId);
        }

        // Insert Record
        [HttpPost, Authorize(Roles = "Student")]
        public int Post(RecordDTO record)
        {
            return _repository.Insert(record);
        }

        // Update Record/5
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public void Put(int id, RecordDTO record)
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
