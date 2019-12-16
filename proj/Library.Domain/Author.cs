using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class Author
    {
        public Author(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
    }
}
