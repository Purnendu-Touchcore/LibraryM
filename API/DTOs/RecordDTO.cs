namespace API.DTOs
{
    public class RecordDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? Status { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int FineAmount { get; set; }
        public int UserId { get; set; }
        public string? Email { get; set; }
        public int MaxRows { get; set; }
    }
}