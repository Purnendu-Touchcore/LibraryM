using API.Models;

namespace API.Interfaces
{
  public interface IRecordRepository
  {
      int Insert(Record record);
      IEnumerable<Record> GetRecords();
      IEnumerable<Record> GetRecordsByPageNo(int pageSize, int currentPage);
      IEnumerable<Record> GetRecordsById(int? id);
      IEnumerable<Record> GetStatus(int UserId);
      void UpdateRecordById(int? id, Record record);
      void DeleteRecord(int? id);
  }
}