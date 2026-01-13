using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class Person : EntityBase
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public string? Phone { get; private set; }

        public ICollection<Employee> Employees { get; private set; } = new List<Employee>();
        public User? User { get; private set; } // 1-1

        private Person() { }

        public Person(string firstName, string lastName, string email, string? phone = null)
        {
            UpdateName(firstName, lastName);
            UpdateEmail(email);
            Phone = phone;
        }

        public Person UpdateName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            return this;
        }

        public Person UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            Email = email.Trim();
            return this;
        }
    }
}
