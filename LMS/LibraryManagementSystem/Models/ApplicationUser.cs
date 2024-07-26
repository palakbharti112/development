using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }
}
