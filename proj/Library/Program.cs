﻿namespace Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Library.Application;
    using Library.Domain;
    using Library.Managers;

    public class Program
    {
        private static RentalController rental;

        private static ConsoleColor titleBackColor = ConsoleColor.Black;
        private static ConsoleColor titleFontColor = ConsoleColor.Red;

        private static ConsoleColor noticeBackColor = ConsoleColor.Green;
        private static ConsoleColor noticeFontColor = ConsoleColor.Black;

        private static ConsoleColor infoBackColor = ConsoleColor.Green;
        private static ConsoleColor infoFontColor = ConsoleColor.Black;

        private static ConsoleColor choiceBackColor = ConsoleColor.White;
        private static ConsoleColor choiceFontColor = ConsoleColor.Black;

        private static ConsoleColor menuBackColor = ConsoleColor.Blue;
        private static ConsoleColor menuFontColor = ConsoleColor.White;

       

        public static void Main(string[] args)
        {
            AdminManager adminModel = new AdminManager();
            BookManager booksModel = new BookManager();
            ReaderManager readersModel = new ReaderManager();
            RentalManager rentalModel = new RentalManager();
            rental = new RentalController("server=localhost;port=3306;uid=root;pwd=root;database=Library", booksModel, rentalModel, readersModel, adminModel);
            while (true)
            {
                PrintStartInfo();
            }
        }

        public static void PrintStartInfo()
        {
            Console.Clear();
            List<string> commands = new List<string>() { "Rent", "Return", "Add*", "Remove*", "Get reader*", "Register" };
            Console.BackgroundColor = menuBackColor;
            Console.ForegroundColor = menuFontColor;
            Console.WriteLine("|   Esc - quit   | Enter - choose| Up, Down arrows - navigate |");
            Console.BackgroundColor = titleBackColor;
            Console.ForegroundColor = titleFontColor;
            Console.WriteLine("|                             MENU                            |");
            Console.ResetColor();
            int chosenCommandIndex = 0;
            int index = 0;
            for (int i = 0; i < commands.Count; i++)
            {
                if (index == chosenCommandIndex)
                {
                    Console.BackgroundColor = choiceBackColor;
                    Console.ForegroundColor = choiceFontColor;
                }
                Console.WriteLine("> " + commands[i]);
                Console.ResetColor();
                index++;
            }
            Console.BackgroundColor = titleBackColor;
            Console.ForegroundColor = titleFontColor;
            Console.WriteLine("(*-admin required)");
            Console.ResetColor();
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                ConsoleKey key = keyInfo.Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (chosenCommandIndex > 0)
                    {
                        chosenCommandIndex--;
                    }
                }

                if (key == ConsoleKey.DownArrow)
                {
                    if (chosenCommandIndex < commands.Count - 1)
                    {
                        chosenCommandIndex++;
                    }
                }

                if (key == ConsoleKey.Enter)
                {
                    break;
                }

                if (key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Environment.Exit(0);
                }

                Console.Clear();
                Console.BackgroundColor = menuBackColor;
                Console.ForegroundColor = menuFontColor;
                Console.WriteLine("|   Esc - quit   | Enter - choose| Up, Down arrows - navigate |");
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("|                             MENU                            |");
                Console.ResetColor();
                index = 0;
                foreach (string command in commands)
                {
                    if (index == chosenCommandIndex)
                    {
                        Console.BackgroundColor = choiceBackColor;
                        Console.ForegroundColor = choiceFontColor;
                    }
                    Console.WriteLine("> " + command);
                    Console.ResetColor();
                    index++;
                }
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("(*-admin required)");
                Console.ResetColor();

            }

            Console.Clear();
            Console.WriteLine("Proceeding");
            switch (chosenCommandIndex)
            {
                case 0:
                    ProceedToRent();
                    break;
                case 1:
                    ProceedToReturn();
                    break;
                case 2:
                    ProceedToAdd();
                    break;
                case 3:
                    ProceedToRemove();
                    break;
                case 4:
                    ProceedToGetReader();
                    break;
                case 5:
                    ProceedToRegister();
                    break;
            }
        }

        public static void ProceedToRent()
        {
            string username = string.Empty;
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("----RENT BOOK----");
                Console.ResetColor();
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
                Console.BackgroundColor = menuBackColor;
                Console.ForegroundColor = menuFontColor;
                Console.WriteLine("|   Esc - exit   | Enter - choose| Alt+F - search | Up, Down arrows - navigate |");
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("|                                  RENT BOOK                                   |");
                Console.BackgroundColor = infoBackColor;
                Console.ForegroundColor = infoFontColor;
                Console.WriteLine(">User: " + user.Name);
                Console.ResetColor();
                List<Book> books = rental.GetAllNonBookedBooks().GroupBy(book => book.Name).Select(g => g.First()).ToList();
                Console.WriteLine(books.Count != 0 ? ">Available books: " : "No books available");
                Book book = null;
                int chosenBookIndex = 0;
                string bookname = string.Empty;
                if (books.Count != 0)
                {
                    OptimisedListRender(chosenBookIndex, books);
                    while (true)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey();
                        ConsoleKey key = keyInfo.Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            if (chosenBookIndex > 0)
                            {
                                chosenBookIndex--;
                            }
                        }

                        if (key == ConsoleKey.DownArrow)
                        {
                            if (chosenBookIndex < books.Count - 1)
                            {
                                chosenBookIndex++;
                            }
                        }

                        if (key == ConsoleKey.Enter)
                        {
                            break;
                        }

                        if (key == ConsoleKey.Escape)
                        {
                            return;
                        }

                        if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0 & key == ConsoleKey.F)
                        {
                            if (ProceedToFindRent(user))
                            {
                                return;
                            }

                        }

                        Console.Clear();
                        Console.BackgroundColor = menuBackColor;
                        Console.ForegroundColor = menuFontColor;
                        Console.WriteLine("|   Esc - exit   | Enter - choose| Alt+F - search | Up, Down arrows - navigate |");
                        Console.BackgroundColor = titleBackColor;
                        Console.ForegroundColor = titleFontColor;
                        Console.WriteLine("|                                  RENT BOOK                                   |");
                        Console.BackgroundColor = infoBackColor;
                        Console.ForegroundColor = infoFontColor;
                        Console.WriteLine(">User: " + user.Name);
                        Console.ResetColor();
                        Console.WriteLine(books.Count != 0 ? ">Available books: " : "No books available");
                        OptimisedListRender(chosenBookIndex, books);
                    }

                    book = books[chosenBookIndex];
                    if (book != null)
                    {
                        Console.Clear();
                        Console.BackgroundColor = titleBackColor;
                        Console.ForegroundColor = titleFontColor;
                        Console.WriteLine("----RENT BOOK----");
                        Console.BackgroundColor = infoBackColor;
                        Console.ForegroundColor = infoFontColor;
                        Console.WriteLine(">User: " + user.Name + "\n" + ">Book: " + book.Name);
                        Console.ResetColor();
                        Console.WriteLine(rental.TryRentBook(book, user) ? "Rented successfully" : "Error, coludn't rent");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No such book available");
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

            Console.BackgroundColor = titleBackColor;
            Console.ForegroundColor = titleFontColor;
            Console.WriteLine("----RETURN BOOK----");
            Console.ResetColor();

            string username = string.Empty;
            while (true)
            {
                Console.Clear();
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("|                   RETURN BOOK                   |");
                Console.ResetColor();
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

            int chosenBookIndex = 0;
            Console.Clear();
            Console.BackgroundColor = menuBackColor;
            Console.ForegroundColor = menuFontColor;
            Console.WriteLine("|   Esc - exit   | Enter - choose| Up, Down arrows - navigate |");
            Console.BackgroundColor = titleBackColor;
            Console.ForegroundColor = titleFontColor;
            Console.WriteLine("|                         RENT BOOK                           |");
            Console.BackgroundColor = infoBackColor;
            Console.ForegroundColor = infoFontColor;
            Console.WriteLine(">User: " + user.Name);
            Console.ResetColor();
            List<Book> books = rental.GetUserBooks(user);
            Console.WriteLine(books.Count != 0 ? ">Your books: " : "You haven't rent any books yet");
            if (books.Count != 0)
            {
                OptimisedListRender(chosenBookIndex, books);
                Guid bookid = Guid.Empty;
                while (true)
                {

                    ConsoleKey key = Console.ReadKey().Key;


                    if (key == ConsoleKey.UpArrow)
                    {
                        if (chosenBookIndex > 0)
                        {
                            chosenBookIndex--;
                        }
                    }

                    if (key == ConsoleKey.DownArrow)
                    {
                        if (chosenBookIndex < books.Count - 1)
                        {
                            chosenBookIndex++;
                        }
                    }

                    if (key == ConsoleKey.Enter)
                    {
                        break;
                    }

                    if (key == ConsoleKey.Escape)
                    {
                        return;
                    }

                    Console.Clear();
                    Console.BackgroundColor = menuBackColor;
                    Console.ForegroundColor = menuFontColor;
                    Console.WriteLine("|   Esc - exit   | Enter - choose| Up, Down arrows - navigate |");
                    Console.BackgroundColor = titleBackColor;
                    Console.ForegroundColor = titleFontColor;
                    Console.WriteLine("|                         RENT BOOK                           |");
                    Console.ResetColor();
                    Console.BackgroundColor = infoBackColor;
                    Console.ForegroundColor = infoFontColor;
                    Console.WriteLine(">User: " + user.Name);
                    Console.ResetColor();
                    Console.WriteLine(books.Count != 0 ? ">Available books: " : "No books available");
                    OptimisedListRender(chosenBookIndex, books);

                }

                bookid = books[chosenBookIndex].Id;
                Console.Clear();
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("----RETURN BOOK----");
                Console.BackgroundColor = infoBackColor;
                Console.ForegroundColor = infoFontColor;
                Console.WriteLine(">User: " + user.Name);
                Console.ResetColor();
                Book b = rental.TryGetBookById(bookid);
                if (b == null)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("----RETURN BOOK----");
                    Console.ResetColor();
                    Console.WriteLine("\n" + "No such book" + "\n" + "\n" + "Press any key to proceed...");
                    while (true)
                    {
                        if (Console.ReadKey() != null)
                        {
                            break;
                        }
                    }

                    return;
                }

                Console.BackgroundColor = infoBackColor;
                Console.ForegroundColor = infoFontColor;
                Console.WriteLine(">Book: " + b.Name);
                Console.ResetColor();
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
            string username;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----ADD BOOK----" + "\n" + "\n" + ">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Admin admin = rental.TryPickAdminByUsername(username);
            if (admin == null)
            {
                NoAdminCase();
                return;
            }

            Console.Clear();
            Console.WriteLine("----ADD BOOK----");
            Console.WriteLine();
            Console.WriteLine("Hi, Admin " + admin.Name + ", id: " + admin.Id + "\n" + ">Input book name: ");
            string bookname = Console.ReadLine();
            Console.WriteLine("OK, now input authors delimited by coma");
            string astr = Console.ReadLine();
            List<Author> authors = new List<Author>();
            foreach (string str in astr.Split(","))
            {
                authors.Add(new Author(Guid.NewGuid(), str));
            }

            Console.WriteLine(rental.TryAddBook(admin.CreateBook(Guid.NewGuid(), bookname, authors)) ? "OK, added a new book" : "Book already exists");
            Console.WriteLine("\n" + "Press any key to proceed to menu...");
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
                Console.WriteLine("----REMOVE BOOK----" + "\n" + "\n" + ">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Admin admin = rental.TryPickAdminByUsername(username);
            if (admin == null)
            {
                NoAdminCase();
                return;
            }

            Console.Clear();
            Console.WriteLine("----REMOVE BOOK----" + "\n" + "\n" + "Hi, Admin " + admin.Name + ", id: " + admin.Id);
            IReadOnlyCollection<Book> books = rental.GetAllBooks();
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
                    Console.WriteLine("----RETURN BOOK----" + "\n" + "\n" + "No such book" + "\n" + "\n" + "Press any key to proceed...");
                    while (true)
                    {
                        if (Console.ReadKey() != null)
                        {
                            break;
                        }
                    }

                    return;
                }

                Console.WriteLine(rental.TryRemoveBook(book) ? "OK, removed " + book.Name + "(id: " + book.Id + ")" : "Can't remove book, already in use");
            }

            Console.WriteLine("\n" + "Press any key to proceed to menu...");
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
                Console.WriteLine("----GET READER OF BOOK----" + "\n" + "\n" + ">Type your username");
                username = Console.ReadLine();
                if (!username.Trim().Equals(string.Empty))
                {
                    break;
                }
            }

            Admin admin = rental.TryPickAdminByUsername(username);
            if (admin == null)
            {
                NoAdminCase();
                return;
            }

            Console.Clear();
            Console.WriteLine("----GET READER OF BOOK----" + "\n" + "\n" + "Hi, Admin " + admin.Name + ", id: " + admin.Id);
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
                    Console.WriteLine("----RETURN BOOK----" + "\n" + "\n" + "No such book" + "\n" + "\n" + "Press any key to proceed...");
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
                Console.WriteLine("_________" + "\n" + "Reader of " + b.Name + "(id: " + b.Id + ") is " + reader.Name + "(id: " + reader.Id + ")");
            }


            Console.WriteLine("\n" + "Press any key to proceed to menu...");
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
            Console.WriteLine("----REGISTER----" + "\n" + "\n" + "Type username: ");
            string username = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("----REGISTER----" + "\n");
            Guid newId = Guid.NewGuid();
            bool result = rental.TryRegisterUser(new Reader(newId, username));
            if (result)
            {
                Console.WriteLine("Ok, your username will be: " + username);
            }
            Console.WriteLine(result ? "Successfully registered" : "Problem registering, maybe user exists");

            Console.WriteLine("\n" + "Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        public static bool ProceedToFindRent(Reader user)
        {

            Book res = null;
            Console.Clear();
            Console.BackgroundColor = menuBackColor;
            Console.ForegroundColor = menuFontColor;
            Console.WriteLine("|   Esc - exit   | Enter - choose| Up, Down arrows - navigate |");
            Console.BackgroundColor = titleBackColor;
            Console.ForegroundColor = titleFontColor;
            Console.WriteLine("|                         FIND BOOK                           |");
            Console.BackgroundColor = noticeBackColor;
            Console.ForegroundColor = noticeFontColor;
            Console.WriteLine("Input word to search for: ");
            Console.ResetColor();
            string input = string.Empty;
            int index = 0;
            int selectedIndex = 0;

            List<Book> foundBooks = new List<Book>();

            while (true)
            {
                foundBooks = rental.FindBooks(input).GroupBy(book => book.Name).Select(g => g.First()).ToList();
                index = 0;
                Console.Clear();
                Console.BackgroundColor = menuBackColor;
                Console.ForegroundColor = menuFontColor;
                Console.WriteLine("|   Esc - exit   | Enter - choose| Up, Down arrows - navigate |");
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("|                         FIND BOOK                           |");
                Console.BackgroundColor = noticeBackColor;
                Console.ForegroundColor = noticeFontColor;
                Console.WriteLine(">Input word to search for: ");
                Console.ResetColor();
                Console.BackgroundColor = choiceBackColor;
                Console.ForegroundColor = choiceFontColor;
                Console.WriteLine(">" + input);
                Console.ResetColor();
                Console.BackgroundColor = noticeBackColor;
                Console.ForegroundColor = noticeFontColor;
                Console.WriteLine(">Select book using arrows");
                Console.ResetColor();
                Console.WriteLine(foundBooks.Count != 0 ? ">Available books: " : "No books available");
                if (foundBooks.Count > 0) OptimisedListRender(selectedIndex, foundBooks);
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                ConsoleKey key = keyInfo.Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if (selectedIndex < foundBooks.Count - 1)
                    {
                        selectedIndex++;
                    }
                }
                else if (key == ConsoleKey.Enter)
                {

                    break;
                }
                else if (key == ConsoleKey.Escape)
                {
                    return false;
                }
                else if (key == ConsoleKey.Backspace)
                {
                    selectedIndex = 0;
                    if (!string.IsNullOrEmpty(input))
                    {
                        input = input.Substring(0, input.Length - 1);
                    }
                }
                else
                {
                    if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                    {
                        input += key.ToString();
                    }
                    else
                    {
                        input += key.ToString().ToLower();
                    }
                }
            }

            if (foundBooks.Count <= 0)
            {
                Console.WriteLine();
                Console.WriteLine("Noting found, press any key to return...");
                while (true)
                {
                    if (Console.ReadKey() != null)
                    {
                        break;
                    }
                }

                return false;
            }

            res = foundBooks[selectedIndex];
            if (res != null)
            {
                Console.Clear();
                Console.Clear();
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.Clear();
                Console.BackgroundColor = menuBackColor;
                Console.ForegroundColor = menuFontColor;
                Console.WriteLine("|   Esc - exit   | Enter - choose| Up, Down arrows - navigate |");
                Console.BackgroundColor = titleBackColor;
                Console.ForegroundColor = titleFontColor;
                Console.WriteLine("|                         FIND BOOK                           |");
                Console.BackgroundColor = infoBackColor;
                Console.ForegroundColor = infoFontColor;
                Console.WriteLine(">User: " + user.Name + "\n" + ">Book: " + res.Name);
                Console.ResetColor();
                Console.WriteLine(rental.TryRentBook(res, user) ? "Rented successfully" : "Error, coludn't rent");
                Console.WriteLine("\n" + "Press any key to proceed...");
                while (true)
                {
                    if (Console.ReadKey() != null)
                    {
                        break;
                    }
                }

                return true;
            }
            else
            {
                Console.WriteLine("No such book available");
                return false;
            }
        }

        private static void NoUserCase()
        {
            Console.Clear();
            Console.WriteLine("No such user, please register" + "\n" + "\n" + "Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        private static void NoAdminCase()
        {
            Console.Clear();
            Console.WriteLine("No such admin found with this username, please check input" + "\n" + "\n" + "Press any key to proceed to menu...");
            while (true)
            {
                if (Console.ReadKey() != null)
                {
                    break;
                }
            }
        }

        private static void OptimisedListRender(int pos, List<Book> list)
        {
            string output = string.Empty;
            int index = 0;
            while (index < pos)
            {
                output += list[index].Name + ", by ";
                foreach (Author author in list[index].Authors)
                {
                    output += author.Name + ", ";
                }

                output = output.Substring(0, output.Length - 2);
                output += "\n";
                index++;
            }

            Console.Write(output);
            Console.BackgroundColor = choiceBackColor;
            Console.ForegroundColor = choiceFontColor;
            output = string.Empty;
            output += list[pos].Name + ", by ";
            foreach (Author author in list[pos].Authors)
            {
                output += author.Name + ", ";
            }

            output = output.Substring(0, output.Length - 2);
            Console.WriteLine(output);
            Console.ResetColor();
            output = string.Empty;
            index = pos + 1;
            while (index > pos && index < list.Count)
            {
                output += list[index].Name + ", by ";
                foreach (Author author in list[index].Authors)
                {
                    output += author.Name + ", ";
                }

                output = output.Substring(0, output.Length - 2);
                output += "\n";
                if (index == list.Count - 1)
                {
                    break;
                }

                index++;
            }

            Console.Write(output);
        }
    }
}
