using LMS.Data;
using LMS.Domain;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LMS.Library
{
    internal class LibraryManagement
    {
        static void Main(string[] args)
        {
            LibraryDbContext ctx = new LibraryDbContext();
            //var Book = new Book()
            //{
            //    Title = "Test Book 1",
            //    Author = "Test Author",
            //    Subject = "Test Subject",
            //    PublicationDate = "2024",
            //    DDC = "500",
            //    ISBN = 1651314856,
            //    AddedDate = DateTime.Now
            //};
            //ctx.Books.Add(Book);
            //ctx.SaveChanges();

            //var Member = new Member()
            //{

            //    FullName = "Test Usewfr 2",
            //    Address = "Test Addrwrfess 2, Test City, Test Country",
            //    PhoneNumber = "12345678wf9990212",
            //    EmailAddress = "TestEwfmail2@gmail.com",
            //    JoinDate = DateTime.Now
            //};
            //ctx.Members.Add(Member);
            //ctx.SaveChanges();

            //var selectedBook = ctx.Books.Where(b=>b.Id==3).First();
            //var selectedMember = ctx.Members.Where(m => m.Id == 2).First();

            //selectedMember.BorrowedBookId = selectedBook.Id;
            //selectedMember.Book = selectedBook;
            //selectedMember.BorrowDate = DateTime.Now;

            //selectedBook.Borrower = selectedMember;
            //selectedBook.BorrowerId = selectedMember.Id;

            //ctx.SaveChanges();

            //var theBook = ctx.Books.Include(b=>b.Borrower).First();
            //Console.WriteLine($"Book Title:{theBook.Title}\nBook Author:{theBook.Author}\nPubslished On:{theBook.PublicationDate}\nCurrently Borrowed By:{theBook.Borrower.FullName}");


            while (true)
            {
                bool exit = false;
                Console.WriteLine("\nWelcome to Library Management System!");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\n1:Add a new book to the library\n2:Lend a book to a member\n3:Report book return by a borrower\n4:Add a new member\n5:Remove a member\n6:Remove a book\n7:View all books\n0:Exit");
                Console.WriteLine("\n");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Enter book title:");
                        string title = Console.ReadLine();
                        Console.WriteLine("Enter book author:");
                        string author = Console.ReadLine();
                        Console.WriteLine("Enter subject of the book:");
                        string subject = Console.ReadLine();
                        Console.WriteLine("Enter publication date or enter None:");
                        string? pd = NoneToNull(Console.ReadLine());
                        Console.WriteLine("Enter book Call Number or enter None:");
                        string? bcn = NoneToNull(Console.ReadLine());
                        Console.WriteLine("Enter DDC or enter None:");
                        string? ddc = NoneToNull(Console.ReadLine());
                        Console.WriteLine("Enter ISBN:");
                        int isbn = Convert.ToInt32(Console.ReadLine());
                        AddBook(title, author, subject, pd, bcn, ddc, isbn);
                        Console.WriteLine("Book Added Successfully!");
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
                        Console.WriteLine("Enter full name:");
                        string fullName = Console.ReadLine();
                        Console.WriteLine("Enter address:");
                        string address = Console.ReadLine();
                        Console.WriteLine("Enter phone number");
                        string phone = Console.ReadLine();
                        Console.WriteLine("Enter Email Address:");
                        string email = Console.ReadLine();
                        AddMember(fullName, address, phone, email);
                        Console.WriteLine("Member added successfully!");
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
                Console.WriteLine($"\nId:{theBook.Id}\nTitle:{theBook.Title}\nAuthor:{theBook.Author}\nSubject:{theBook.Subject}\nPublished On:{theBook.PublicationDate}\nCallNumber:{theBook.CallNumber}\nDDC:{theBook.DDC}\nISBN:{theBook.DDC}\nAdded On:{theBook.AddedDate}\nBorrower ID:{theBook.BorrowerId}\n");
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
