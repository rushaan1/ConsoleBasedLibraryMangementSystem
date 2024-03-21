using LMS.Data;
using LMS.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LMS.Library
{
    internal class LibraryManagement
    {
        static string username;
        static void Main(string[] args)
        {
            LibraryDbContext ctx = new LibraryDbContext();

            //var salt = PasswordHelper.GenerateSalt();
            //var pHash = PasswordHelper.HashPassword("ad1212", salt);

            //var admin = new Administrator()
            //{
            //    FullName = "Admin1FName",
            //    PhoneNumber = "56457513354",
            //    Email = "admin1@gmail.com",
            //    Address = "admin sample address, sample city, sample country",
            //    Username = "admin1",
            //    PasswordHash = pHash,
            //    Salt = salt,
            //    JoinDate = DateTime.Now
            //};

            //ctx.Administrators.Add(admin);
            //ctx.SaveChanges();

            while (true) 
            {
                Console.WriteLine("Login to Library Mangement System\n\nEnter Username:");
                username = Console.ReadLine(); 
                Console.WriteLine("\nEnter Password:");
                string pwd = Console.ReadLine();
                if (VerifyPassword(username, pwd)) 
                {
                    break;
                }
                Console.WriteLine("\nIncorrect username or password!\n");
            }

            while (true)
            {
                bool exit = false;
                Console.WriteLine("\n\nWelcome to Library Management System!");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\n1:Add a new book to the library\n2:Lend a book to a member\n3:Report book return by a borrower\n4:Add a new member\n5:Remove a member\n6:Remove a book\n7:View all books\n8:Update a book\n9:Update a member\n10:View All Members\n11:Add a new administrator\n12:Update administrator details\n0:Exit");
                Console.WriteLine("\n");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.WriteLine("\nEnter book title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("\nEnter book author:");
                        string author = Console.ReadLine();
                        Console.WriteLine("\nEnter subject of the book:");
                        string subject = Console.ReadLine();
                        Console.WriteLine("\nEnter publication date or enter None:");
                        string? pd = NoneToNull(Console.ReadLine());
                        Console.WriteLine("\nEnter book Call Number or enter None:");
                        string? bcn = NoneToNull(Console.ReadLine());
                        Console.WriteLine("\nEnter DDC or enter None:");
                        string? ddc = NoneToNull(Console.ReadLine());
                        Console.WriteLine("\nEnter ISBN:");
                        int isbn = Convert.ToInt32(Console.ReadLine());
                        AddBook(title, author, subject, pd, bcn, ddc, isbn);
                        Console.WriteLine("\nBook Added Successfully!\n");
                        break;
                    case 2:
                        Console.WriteLine("Enter phone number of member to whom book is to be lended:");
                        string ph = Console.ReadLine();
                        Console.WriteLine("Available Books:");
                        DisplayAllBookRecords(true);
                        Console.WriteLine("\nEnter book id:");
                        int bookId = Convert.ToInt32(Console.ReadLine());
                        int status = LendBookTo(bookId, ph);
                        if (status == 1)
                        {
                            Console.WriteLine("That book is already borrowed by someone else!");
                        }
                        else if (status == 2)
                        {
                            Console.WriteLine("That person already has borrowed a book!");
                        }
                        else
                        {
                            Console.WriteLine("Successfully book lent!");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Enter phone number of the member who is returning their book:");
                        string phn = Console.ReadLine();
                        if (ReturnBookFrom(phn))
                        {
                            Console.WriteLine("Success!");
                        }
                        else
                        {
                            Console.WriteLine("No active borrowed books found for the provided member!");
                        }
                        break;
                    case 4:
                        Console.WriteLine("\nEnter full name:");
                        string fullName = Console.ReadLine();
                        Console.WriteLine("\nEnter address:");
                        string address = Console.ReadLine();
                        Console.WriteLine("\nEnter phone number");
                        string phone = Console.ReadLine();
                        Console.WriteLine("\nEnter Email Address:");
                        string email = Console.ReadLine();
                        AddMember(fullName, address, phone, email);
                        Console.WriteLine("\nMember added successfully!\n");
                        break;
                    case 5:
                        Console.WriteLine("Enter the phone number of the member to be removed!");
                        string p = Console.ReadLine();
                        if (RemoveMember(p))
                        {
                            Console.WriteLine("Member removed successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Unable to remove as that member has a book associated with them!");
                        }
                        break;
                    case 6:
                        Console.WriteLine("\nAvailable books:");
                        DisplayAllBookRecords(true);
                        Console.WriteLine("Enter the id of the book to be removed!");
                        int bid = Convert.ToInt32(Console.ReadLine());
                        if (RemoveBook(bid))
                        {
                            Console.WriteLine("Book removed successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Unable to remove as that book is currently borrowed by a member!");
                        }
                        break;
                    case 7:
                        Console.WriteLine("All books:");
                        DisplayAllBookRecords(false);
                        break;
                    case 8:
                        Console.WriteLine("\nAll books:");
                        DisplayAllBookRecords(false);
                        Console.WriteLine("Which book do you wanna update? Enter id:");
                        int bid_2 = Convert.ToInt32(Console.ReadLine());
                        var book = ctx.Books.Where(b => b.Id == bid_2).First();
                        while (true) 
                        {
                            bool exitbe = false;
                            Console.WriteLine("\nWhat do you want to update about the book?\n1:Title\n2:Author\n3:Subject\n4:Publication Date\n5:Call Number\n6:DDC\n7:ISBN\n8:exit\n");
                            int uoption = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("\n");
                            switch (uoption) 
                            {
                                case 1:
                                    Console.WriteLine("Enter new title:");
                                    string ntitle = Console.ReadLine();
                                    book.Title = ntitle;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated title!");
                                    break;
                                case 2:
                                    Console.WriteLine("Enter new auhtor:");
                                    string nauthor = Console.ReadLine();
                                    book.Author = nauthor;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated author!");
                                    break;
                                case 3:
                                    Console.WriteLine("\nEnter new subject:");
                                    string nsubject = Console.ReadLine();
                                    book.Subject = nsubject;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated subject!");
                                    break;
                                case 4:
                                    Console.WriteLine("Enter new publication date:");
                                    string npd = Console.ReadLine();
                                    book.PublicationDate = npd;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated publication date!");
                                    break;
                                case 5:
                                    Console.WriteLine("Enter new call number:");
                                    string ncn = Console.ReadLine();
                                    book.CallNumber = ncn;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated call number!");
                                    break;
                                case 6:
                                    Console.WriteLine("Enter new title:");
                                    string nddc = Console.ReadLine();
                                    book.DDC = nddc;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated DDC!");
                                    break;
                                case 7:
                                    Console.WriteLine("Enter new ISBN:");
                                    int nisbn = Convert.ToInt32(Console.ReadLine());
                                    book.ISBN = nisbn;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated ISBN!");
                                    break;
                                case 8:
                                    exitbe = true;
                                    break;
                            }
                            if (exitbe) 
                            {
                                break;
                            }
                        }
                        break;
                    case 9:
                        Console.WriteLine("\nAll Members:");
                        DisplayAllMemberRecords();
                        Console.WriteLine("Which member do you wanna update? Enter id:");
                        int mid = Convert.ToInt32(Console.ReadLine());
                        var member = ctx.Members.Where(m => m.Id == mid).First();
                        while (true)
                        {
                            bool exitme = false;
                            Console.WriteLine("\nWhat do you want to update about the member?\n1:Full Name\n2:Address\n3:Phone Number\n4:Email Address\n5:exit\n");
                            int uoption = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("\n");
                            switch (uoption)
                            {
                                case 1:
                                    Console.WriteLine("Enter new full name:");
                                    string nfn = Console.ReadLine();
                                    member.FullName = nfn;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated full name!");
                                    break;
                                case 2:
                                    Console.WriteLine("Enter new address:");
                                    string nad = Console.ReadLine();
                                    member.Address = nad;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated address!");
                                    break;
                                case 3:
                                    Console.WriteLine("Enter new phone number:");
                                    string npn = Console.ReadLine();
                                    member.PhoneNumber = npn;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated phone number!");
                                    break;
                                case 4:
                                    Console.WriteLine("Enter new email address:");
                                    string nea = Console.ReadLine();
                                    member.EmailAddress = nea;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated email address!");
                                    break;
                                case 5:
                                    exitme = true;
                                    break;
                            }
                            if (exitme)
                            {
                                break;
                            }
                        }
                        break;
                    case 10:
                        Console.WriteLine("All Members:");
                        DisplayAllMemberRecords();
                        break;


                    case 11:
                        Console.WriteLine("\nEnter full name:");
                        string fullName_a = Console.ReadLine();
                        Console.WriteLine("\nEnter address:");
                        string address_a = Console.ReadLine();
                        Console.WriteLine("\nEnter phone number");
                        string phone_a = Console.ReadLine();
                        Console.WriteLine("\nEnter Email Address:");
                        string email_a = Console.ReadLine();
                        Console.WriteLine("\nEnter username:");
                        string uname = Console.ReadLine();
                        Console.WriteLine("\nEnter password:");
                        string pwd1 = Console.ReadLine();
                        Console.WriteLine("\nEnter password again:");
                        string pwd2 = Console.ReadLine();
                        if (pwd1 != pwd2) 
                        {
                            Console.WriteLine("\nPasswords do not match!\n");
                            break;
                        }
                        AddAdministrator(fullName_a, address_a, phone_a, email_a, uname, pwd2);
                        Console.WriteLine("\nAdministrator added successfully!\n");
                        break;



                    case 12:
                        var admin = ctx.Administrators.Where(a => a.Username == username).First();
                        while (true)
                        {
                            bool exitme = false;
                            Console.WriteLine("\nWhat do you want to update about current logged in administrator?\n1:Full Name\n2:Address\n3:Phone Number\n4:Email Address\n5:Username\n6:Password\n7:exit\n");
                            int uoption = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("\n");
                            switch (uoption)
                            {
                                case 1:
                                    Console.WriteLine("Enter new full name:");
                                    string nfn = Console.ReadLine();
                                    admin.FullName = nfn;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated full name!");
                                    break;
                                case 2:
                                    Console.WriteLine("Enter new address:");
                                    string nad = Console.ReadLine();
                                    admin.Address = nad;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated address!");
                                    break;
                                case 3:
                                    Console.WriteLine("Enter new phone number:");
                                    string npn = Console.ReadLine();
                                    admin.PhoneNumber = npn;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated phone number!");
                                    break;
                                case 4:
                                    Console.WriteLine("Enter new email address:");
                                    string nea = Console.ReadLine();
                                    admin.Email = nea;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated email address!");
                                    break;
                                case 5:
                                    Console.WriteLine("Enter new username:");
                                    string nun = Console.ReadLine();
                                    admin.Username = nun;
                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated username!");
                                    break;
                                case 6:
                                    Console.WriteLine("Enter new password:");
                                    string npwd = Console.ReadLine();
                                    Console.WriteLine("\nEnter new password again:");
                                    string npwd2 = Console.ReadLine();
                                    if (npwd != npwd2) 
                                    {
                                        Console.WriteLine("\nPasswords do not match!");
                                        break;
                                    }
                                    var salt = PasswordHelper.GenerateSalt();
                                    var pHash = PasswordHelper.HashPassword(npwd2, salt);
                                    admin.Salt = salt;
                                    admin.PasswordHash = pHash;

                                    ctx.SaveChanges();
                                    Console.WriteLine("\nUpdated password!");
                                    break;


                                case 7:
                                    exitme = true;
                                    break;
                            }
                            if (exitme)
                            {
                                break;
                            }
                        }
                        break;


                    case 0:
                        exit = true;
                        Console.WriteLine("GoodBye! Exited Library Mangement System.");
                        break;
                }
                if (exit) 
                {
                    break;
                }
                Console.WriteLine("Press anything to continue...");
                Console.ReadKey();
            }
        }

        static void AddAdministrator(string fullName_a, string address_a, string phone_a, string email_a, string uname, string pwd)
        {
            LibraryDbContext ctx = new LibraryDbContext();

            var salt = PasswordHelper.GenerateSalt();
            var pHash = PasswordHelper.HashPassword(pwd, salt);

            var admin = new Administrator()
            {
                FullName = fullName_a,
                PhoneNumber = phone_a,
                Email = email_a,
                Address = address_a,
                Username = uname,
                PasswordHash = pHash,
                Salt = salt,
                JoinDate = DateTime.Now
            };

            ctx.Administrators.Add(admin);
            ctx.SaveChanges();
        }

        static void AddBook(string title, string author, string subject, string? publicationDate, string? callNumber, string? ddc, int isbn) 
        {
            LibraryDbContext ctx = new LibraryDbContext();
            var Book = new Book()
            {
                Title = title,
                Author = author,
                Subject = subject,
                PublicationDate = publicationDate,
                CallNumber = callNumber,
                DDC = ddc,
                ISBN = isbn,
                AddedDate = DateTime.Now
            };
            ctx.Books.Add(Book);
            ctx.SaveChanges();
        }

        static void AddMember(string fname, string address, string ph, string email) 
        {
            LibraryDbContext ctx = new LibraryDbContext();
            var Member = new Member()
            {
                FullName = fname,
                Address = address,
                PhoneNumber = ph,
                EmailAddress = email,
                JoinDate = DateTime.Now
            };
            ctx.Members.Add(Member);
            ctx.SaveChanges();
        }

        static void DisplayAllBookRecords(bool notBorrowed) 
        {
            LibraryDbContext ctx = new LibraryDbContext();
            List<Book> books;
            if (notBorrowed)
            {
                books = ctx.Books.Where(b=>b.BorrowerId==null).ToList();
            }
            else 
            {
                books = ctx.Books.ToList();
            }
            foreach (var theBook in books) 
            {
                Console.WriteLine($"\nId:{theBook.Id}\nTitle:{theBook.Title}\nAuthor:{theBook.Author}\nSubject:{theBook.Subject}\nPublished On:{theBook.PublicationDate}\nCallNumber:{theBook.CallNumber}\nDDC:{theBook.DDC}\nISBN:{theBook.ISBN}\nAdded On:{theBook.AddedDate}\nBorrower ID:{theBook.BorrowerId}\n");
            }
        }

        static void DisplayAllMemberRecords()
        {
            LibraryDbContext ctx = new LibraryDbContext();
            List<Member> members = ctx.Members.ToList();

            foreach (var m in members)
            {
                Console.WriteLine($"\nId:{m.Id}\nFull Name:{m.FullName}\nAddress:{m.Address}\nEmail Address:{m.EmailAddress}\nPhone Number:{m.PhoneNumber}\nJoinDate:{m.JoinDate}\nBorrowed Book Id:{m.BorrowedBookId}\n");
            }
        }

        static int LendBookTo(int bookId, string ph) 
        {
            LibraryDbContext ctx = new LibraryDbContext();
            var selectedBook = ctx.Books.Where(b => b.Id == bookId).First();
            var selectedMember = ctx.Members.Where(m => m.PhoneNumber == ph).First();

            if (selectedBook.BorrowerId != null) 
            {
                return 1;
            }

            if (selectedMember.BorrowedBookId != null) 
            {
                return 2;
            }

            selectedMember.BorrowedBookId = selectedBook.Id;
            selectedMember.Book = selectedBook;
            selectedMember.BorrowDate = DateTime.Now;

            selectedBook.Borrower = selectedMember;
            selectedBook.BorrowerId = selectedMember.Id;

            ctx.SaveChanges();
            return 0;
        }

        static bool ReturnBookFrom(string ph)
        {
            LibraryDbContext ctx = new LibraryDbContext();
            var selectedMember = ctx.Members.Where(m => m.PhoneNumber == ph).First();
            var selectedBook = ctx.Books.Where(b => b.Id == selectedMember.BorrowedBookId).First();

            if (selectedMember.BorrowedBookId == null)
            {
                return false;
            }

            selectedMember.BorrowedBookId = null;
            selectedMember.Book = null;
            selectedMember.BorrowDate = null;

            selectedBook.Borrower = null;
            selectedBook.BorrowerId = null;

            ctx.SaveChanges();
            return true;
        }

        static bool RemoveMember(string ph) 
        {
            LibraryDbContext ctx = new LibraryDbContext();
            var member = ctx.Members.Where(m => m.PhoneNumber == ph).First();
            if (member.BorrowedBookId != null) 
            {
                return false;
            }

            ctx.Members.Remove(member);
            ctx.SaveChanges();
            return true;
        }

        static bool RemoveBook(int bid)
        {
            LibraryDbContext ctx = new LibraryDbContext();
            var book = ctx.Books.Where(b => b.Id == bid).First();
            if (book.BorrowerId != null)
            {
                return false;
            }

            ctx.Books.Remove(book);
            ctx.SaveChanges();
            return true;
        }

        static bool VerifyPassword(string username, string password)
        {
            using (LibraryDbContext ctx = new LibraryDbContext())
            {
                var administrator = ctx.Administrators.FirstOrDefault(a => a.Username == username);
                if (administrator != null)
                {
                    var storedSalt = administrator.Salt;
                    var storedHashedPwd = administrator.PasswordHash;

                    var hashedPwd = PasswordHelper.HashPassword(password, storedSalt);
                    return hashedPwd.SequenceEqual(storedHashedPwd);
                }
                return false; 
            }
        }

        static string? NoneToNull(string value) 
        {
            if (value.ToLower() == "none") 
            { 
                return null; 
            }
            return value;
        }
    }
}
