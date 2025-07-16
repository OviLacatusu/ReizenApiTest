using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reizen.Domain.Models;

namespace Reizen.Domain.Models
{
    public class Country
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Continent Continent { get; private set; }

        private readonly List<Destination> _destinations = new List<Destination>();
        public ReadOnlyCollection<Destination> Destinations => _destinations.AsReadOnly();
        private Country() { }

        public Country (string name, Continent continent)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");
            if (Continent is null)
                throw new ArgumentNullException ("Continent cannot be null");

            Name = name;
            Continent = continent;
        }

        public void UpdateDetails (string name, Continent continent)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");
            if (Continent is null)
                throw new ArgumentNullException ("Continent cannot be null");

            Name = name;
            Continent = continent;
        }

        public void AddDestination (Destination destination)
        {
            if (destination is null)
                throw new ArgumentNullException ("Destination cannot be null");
            if (destination.Country != this)
                throw new InvalidOperationException ("Destination does not belong to this Country");

            _destinations.Add (destination);
        }
    }
}
