using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress {  get; set; }
        public int? BorrowedBookId { get; set; }
        public Book? Book { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
