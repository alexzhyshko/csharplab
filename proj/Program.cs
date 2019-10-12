namespace Library
{
    using System;
    using System.Collections.Generic;
    using Library.Domain;
    using Library.Model;

    public class Program
    {
        private static RentalController rental;

        public static void Main(string[] args)
        {

            Admin admin = new Admin(Guid.NewGuid(), "alex25713");
            AdminModel adminModel = new AdminModel();
            adminModel.TryAdd(admin);

            BookModel booksModel = new BookModel();
            ReaderModel readersModel = new ReaderModel();
            readersModel.TryAdd(new Reader(Guid.NewGuid(), "adad"));
            readersModel.TryAdd(admin);
            RentalModel rentalModel = new RentalModel();

            rental = new RentalController(booksModel, rentalModel, readersModel, adminModel);

            Author author1 = new Author(Guid.NewGuid(), "J.K.Rowling");

            List<Author> authors1 = new List<Author>() { author1};

            Book book1 = new Book(Guid.NewGuid(), "Harry Potter", authors1);

            rental.TryAddBook(book1);

            while (true)
            {
                PrintStartInfo();
            }
        }

        public static void PrintStartInfo()
        {
            Console.Clear();
            List<string> commands = new List<string>() { "Rent", "Return", "Add*", "Remove*", "Get reader*","Register", "Exit" };
            Console.WriteLine(">Select what you want to do(input number of choice)");
            for (int i = 0; i < commands.Count; i++)
            {
                Console.WriteLine(">" + (i + 1) + ". " + commands[i]);
            }
            Console.WriteLine("(*-admin required)");
            char input = GetInput();
            Console.Clear();
            Console.WriteLine("Proceeding");
            switch (input)
            {
                case '1':
                    ProceedToRent();
                    break;
                case '2':
                    ProceedToReturn();
                    break;
                case '3':
                    ProceedToAdd();
                    break;
                case '4':
                    ProceedToRemove();
                    break;
                case '5':
                    ProceedToGetReader();
                    break;
                case '6':
                    ProceedToRegister();
                    break;
                case '7':
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                case 'a':

                    break;
            }
        }

        public static char GetInput()
        {
            try
            {
                return Console.ReadKey().KeyChar;
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Incorrect input, press any key to proceed...");
                while (true)
                {
                    if (Console.ReadKey() != null)
                    {
                        break;
                    }
                }
            }

            return 'a';
        }

        public static void ProceedToRent()
        {
            string username = string.Empty;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----RENT BOOK----");
                Console.WriteLine();
                Console.WriteLine(">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Reader user = rental.TryPickUserByName(username);
            if (user == null)
            {
                NoUserCase();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("----RENT BOOK----");
                Console.WriteLine();
                Console.WriteLine(">User: " + user.Name);
                Console.WriteLine("(userid: " + user.Id + ")");
                List<Book> books = rental.GetAllNonBookedBooks();
                Console.WriteLine(books.Count != 0 ? ">Available books: " : "No books available");
                Book book = null;
                string bookname = string.Empty;
                if (books.Count != 0)
                {
                    foreach (Book b in books)
                    {
                        string text = b.Name + ", by ";
                        foreach (Author a in b.Authors)
                        {

                            text += a.Name + ", ";

                        }

                        text = text.Substring(0, text.Length - 2);
                        text += "(id: " + b.Id + ")";
                        Console.WriteLine(text);
                    }

                    Console.WriteLine(">Type book title");
                    bookname = Console.ReadLine();
                    if (!bookname.Trim().Equals(string.Empty))
                    {
                        book = rental.TryPickBookByName(bookname);
                        if (book != null)
                        {
                            Console.Clear();
                            Console.WriteLine("----RENT BOOK----");
                            Console.WriteLine();
                            Console.WriteLine(">User: " + user.Name);
                            Console.WriteLine("(userid: " + user.Id + ")");
                            Console.WriteLine(">Book: " + book.Name);
                            Console.WriteLine("(bookid: " + book.Id + ")");
                            Console.WriteLine(rental.TryRentBook(book, user) ? "Rented successfully" : "Error, coludn't rent");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("No such book available");
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        public static void ProceedToReturn()
        {
            Console.Clear();
            Console.WriteLine("----RETURN BOOK----");

            string username = string.Empty;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----RETURN BOOK----");
                Console.WriteLine();
                Console.WriteLine(">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Reader user = rental.TryPickUserByName(username);
            if (user == null)
            {
                NoUserCase();
                return;
            }
            Console.Clear();
            Console.WriteLine("----RETURN BOOK----");
            Console.WriteLine();
            Console.WriteLine(">User: " + user.Name);
            Console.WriteLine("(userid: " + user.Id + ")");
            List<Book> books = rental.GetUserBooks(user);
            Console.WriteLine(books.Count != 0 ? ">Your books: " : "You haven't rented any books yet");
            if (books.Count != 0)
            {
                foreach (Book book in books)
                {
                    string text = string.Empty;
                    text += book.Name + "(" + book.Id + "), by ";
                    foreach (Author author in book.Authors)
                    {
                        text += author.Name + ", ";
                    }

                    text = text.Substring(0, text.Length - 2);
                    Console.WriteLine(text);

                }

                Console.WriteLine(">Type book id: ");
                Guid bookid = Guid.Empty;
                while (true)
                {
                    try
                    {
                        bookid = Guid.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Incorrect input, try again");
                    }
                }
                Console.Clear();
                Console.WriteLine("----RETURN BOOK----");
                Console.WriteLine();
                Console.WriteLine(">User: " + user.Name);
                Console.WriteLine("(userid: " + user.Id + ")");
                Book b = rental.TryGetBookById(bookid);
                if (b == null)
                {
                    Console.Clear();
                    Console.WriteLine("----RETURN BOOK----");
                    Console.WriteLine();
                    Console.WriteLine("No such book");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to proceed...");
                    while (true)
                    {
                        if (Console.ReadKey() != null)
                        {
                            break;
                        }
                    }

                    return;
                }

                Console.WriteLine(">Book: " + b.Name);
                Console.WriteLine("(bookid: " + b.Id + ")");
                Console.WriteLine(rental.TryReturnBook(b, user) ? "Returned successfully" : "Error, coludn't return");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to proceed...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        public static void ProceedToAdd()
        {
            Console.Clear();
            Console.WriteLine("----ADD BOOK----");
            string username;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----ADD BOOK----");
                Console.WriteLine();
                Console.WriteLine(">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Reader user = rental.TryPickUserByName(username);
            if (user == null)
            {
                NoUserCase();
                return;
            }

            if (rental.CheckRights(user))
            {
                    Console.Clear();
                    Console.WriteLine("----ADD BOOK----");
                    Console.WriteLine();
                    Console.WriteLine("Hi, Admin " + user.Name + ", id: " + user.Id);
                    Console.WriteLine(">Input book name: ");
                    string bookname = Console.ReadLine();
                    Console.WriteLine("OK, now input authors delimited by coma");
                    string astr = Console.ReadLine();
                    List<Author> authors = new List<Author>();
                    foreach (string str in astr.Split(","))
                    {
                        authors.Add(new Author(Guid.NewGuid(), str));
                    }

                    Console.WriteLine(rental.TryAddBook(new Book(Guid.NewGuid(), bookname, authors)) ? "OK, added a new book" : "Book already exists");
            }
            else
            {
                Console.WriteLine("Admin rights required for this operation");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        public static void ProceedToRemove()
        {

            string username;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----REMOVE BOOK----");

                Console.WriteLine();
                Console.WriteLine(">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Reader user = rental.TryPickUserByName(username);
            if (user == null)
            {
                NoUserCase();
                return;
            }

            if (rental.CheckRights(user))
            {

                Console.Clear();
                Console.WriteLine("----REMOVE BOOK----");
                Console.WriteLine();
                Console.WriteLine("Hi, Admin " + user.Name + ", id: " + user.Id);

                List<Book> books = rental.GetAllBooks();
                Console.WriteLine(books.Count != 0 ? ">Available books: " : "No books available");
                if (books.Count != 0)
                {
                    foreach (Book b in books)
                    {
                        string text = b.Name + ", by ";
                        foreach (Author a in b.Authors)
                        {

                            text += a.Name + ", ";

                        }

                        text = text.Substring(0, text.Length - 2);
                        text += "(id: " + b.Id + ")";
                        Console.WriteLine(text);
                    }

                    Console.WriteLine(">Input book id: ");
                    Guid bookid = Guid.Parse(Console.ReadLine());
                    Book book = rental.TryGetBookById(bookid);
                    if (book == null)
                    {
                        Console.Clear();
                        Console.WriteLine("----RETURN BOOK----");
                        Console.WriteLine();
                        Console.WriteLine("No such book");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to proceed...");
                        while (true)
                        {
                            if (Console.ReadKey() != null)
                            {
                                break;
                            }
                        }

                        return;
                    }

                    Console.WriteLine(rental.TryRemoveBook(book) ? "OK, removed " + book.Name + "(id: "+book.Id + ")" : "Book don't exist");
                }
            }
            else
            {
                Console.WriteLine("Admin rights required for this operation");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        public static void ProceedToGetReader()
        {

            string username;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----GET READER OF BOOK----");

                Console.WriteLine();
                Console.WriteLine(">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Reader user = rental.TryPickUserByName(username);
            if (user == null)
            {
                NoUserCase();
                return;
            }

            if (rental.CheckRights(user))
            {
                Console.Clear();
                Console.WriteLine("----GET READER OF BOOK----");
                Console.WriteLine();
                Console.WriteLine("Hi, Admin " + user.Name + ", id: " + user.Id);
                Dictionary<Book, Reader> result = rental.GetRentBooks();
                Console.WriteLine(result.Count != 0 ? "Rent books: " : "No rent books");
                if (result.Count != 0)
                {
                    foreach (Book book in result.Keys)
                    {
                        Console.WriteLine("Book: " + book.Name + "(id: " + book.Id + ") | Reader: " + result[book].Name + "(id: " + result[book].Id + ")");
                    }

                    Console.WriteLine("Type book id: ");
                    Guid bookid = Guid.Parse(Console.ReadLine());
                    Book b = rental.TryGetBookById(bookid);
                    if (b == null)
                    {
                        Console.Clear();
                        Console.WriteLine("----RETURN BOOK----");
                        Console.WriteLine();
                        Console.WriteLine("No such book");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to proceed...");
                        while (true)
                        {
                            if (Console.ReadKey() != null)
                            {
                                break;
                            }
                        }

                        return;
                    }

                    Reader reader = rental.TryGetReader(b);
                    Console.WriteLine("_________");
                    Console.WriteLine("Reader of " + b.Name + "(id: " + b.Id + ") is " + reader.Name + "(id: " + reader.Id + ")");
                }
            }
            else
            {
                Console.WriteLine("Admin rights required for this operation");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        public static void ProceedToRegister()
        {
            Console.Clear();
            Console.WriteLine("----REGISTER----");
            Console.WriteLine();
            Console.WriteLine("Type username: ");
            string username = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("----REGISTER----");
            Console.WriteLine();
            Guid newId = Guid.NewGuid();
            bool result = rental.TryRegisterUser(new Reader(newId, username));
            if (result)
            {
                Console.WriteLine("Ok, your username will be: " + username); 
                Console.WriteLine("(id: " + newId + ")");
            }
            Console.WriteLine(result?"Successfully registered":"Problem registering, maybe user exists");

            Console.WriteLine();
            Console.WriteLine("Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        private static void NoUserCase()
        {
            Console.Clear();
            Console.WriteLine("No such user, please register");
            Console.WriteLine();
            Console.WriteLine("Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }
    }
}
