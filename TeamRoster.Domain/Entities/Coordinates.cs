using System;

namespace TeamRoster.Domain.Entities
{
    public class Coordinates
    {
        public decimal? Latitude { get; private set; }
        public decimal? Longitude { get; private set; }

        private Coordinates() { }

        public Coordinates(decimal? latitude = null, decimal? longitude = null)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinates Update(decimal? latitude, decimal? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            return this;
        }
    }
}
