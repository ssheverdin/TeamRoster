using System;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class Address
    {
        public string? Street { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public string? PostalCode { get; private set; }
        public string? Country { get; private set; }

        private Address() { }

        public Address(string? street = null, string? city = null, string? state = null, string? postalCode = null, string? country = null)
        {
            Street = string.IsNullOrWhiteSpace(street) ? null : street.Trim();
            City = string.IsNullOrWhiteSpace(city) ? null : city.Trim();
            State = string.IsNullOrWhiteSpace(state) ? null : state.Trim();
            PostalCode = string.IsNullOrWhiteSpace(postalCode) ? null : postalCode.Trim();
            Country = string.IsNullOrWhiteSpace(country) ? null : country.Trim();
        }

        public Address UpdateStreet(string? street) { Street = string.IsNullOrWhiteSpace(street) ? null : street.Trim(); return this; }
        public Address UpdateCity(string? city) { City = string.IsNullOrWhiteSpace(city) ? null : city.Trim(); return this; }
        public Address UpdateState(string? state) { State = string.IsNullOrWhiteSpace(state) ? null : state.Trim(); return this; }
        public Address UpdatePostalCode(string? postalCode) { PostalCode = string.IsNullOrWhiteSpace(postalCode) ? null : postalCode.Trim(); return this; }
        public Address UpdateCountry(string? country) { Country = string.IsNullOrWhiteSpace(country) ? null : country.Trim(); return this; }
    }
}
