using System;
using System.Collections.Generic;

namespace Library.Domain
{
    public class Book
    {
        private List<Author> _authors;

        public List<Author> Authors
        {
            get
            {
                List<Author> res = new List<Author>();
                foreach (Author author in _authors)
                {
                    res.Add(new Author(author.Id, author.Name));
                }

                return res;
            }
        }

        public string Name { get; }

        public Guid Id { get; }

        public Book(Guid identificator, string bookName, List<Author> authors)
        {
            Name = bookName;
            _authors = authors;
            Id = identificator;
        }
    }
}
