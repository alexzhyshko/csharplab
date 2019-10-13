using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class Author : Person
    {
        public Author(Guid id, string name)
            : base(id, name)
        {
        }
    }
}
