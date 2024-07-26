using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }
}
