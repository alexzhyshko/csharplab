using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class Person
    {
        public string Name { get; private set; }

        public Guid Id { get; private set; }

        public Person(Guid identificator, string userName)
        {
            Name = userName;
            Id = identificator;
        }
    }
}
