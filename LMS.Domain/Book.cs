using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public string? PublicationDate { get; set; }
        public string? CallNumber { get; set; }
        public string? DDC { get; set; }
        public int ISBN { get; set; }
        public int? BorrowerId { get; set; }
        public Member? Borrower { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
