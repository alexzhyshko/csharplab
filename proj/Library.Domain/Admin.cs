using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class Admin : Reader
    {
        public Admin(Guid id, string name)
            : base(id, name)
        {
        }

        public Book CreateBook(Guid identificator, string name, List<Author> authors)
        {
            return new Book(identificator, name, authors);
        }
    }
}
