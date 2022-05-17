using API.Models;
using API.DTOs;

namespace API.Interfaces
{
  public interface IRecordRepository
  {
      int Insert(RecordDTO record);
      IEnumerable<RecordDTO> GetRecordsByPageNo(string bookStatus, string email, int pageSize, int pageNumber);
      IEnumerable<RecordDTO> GetStatus(int UserId);
      void UpdateRecordById(int? id, RecordDTO record);
      void DeleteRecord(int? id);
  }
}