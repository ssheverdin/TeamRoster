using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class User : EntityBase
    {
        public int PersonId { get; private set; }
        public Person Person { get; private set; } = default!;

        public string UserName { get; private set; } = default!;
        public bool IsLocked { get; private set; }

        private User() { }

        public User(Person person, string userName)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            PersonId = person.Id;

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.", nameof(userName));

            UserName = userName.Trim();
        }

        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;
    }
}
