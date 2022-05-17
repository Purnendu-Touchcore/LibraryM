using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string? Author {get;set;}
        public string? Description {get;set;}
        public string? CoverImage {get;set;} 
        public string? Publisher {get;set;}
        public int ISBN {get;set;}
        public DateTime? PublicationDate {get;set;}
        public int Pages {get;set;}
        [NotMapped]
        public int MaxRows { get; set; }
        
    }
}
