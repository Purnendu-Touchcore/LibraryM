namespace API.Models
{
    public class Record
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? Status { get; set; }
        public string? IssuedDate { get; set; }
        public string? DueDate { get; set; }
        public string? ReturnDate { get; set; }
        public int FineAmount { get; set; }
        public int UserId { get; set; }
        public string? Email { get; set; }
        public int MaxRows { get; set; }


    }
}
