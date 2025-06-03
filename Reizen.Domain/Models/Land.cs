using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Land
    {
        public int Id { get; private set; }
        public string Naam { get; private set; }
        public Werelddeel Werelddeel { get; private set; }

        private readonly List<Bestemming> _bestemmingen = new List<Bestemming>();
        public ReadOnlyCollection<Bestemming> Bestemmingen => _bestemmingen.AsReadOnly();
        private Land() { }

        public Land (string name, Werelddeel werelddeel)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");
            if (werelddeel is null)
                throw new ArgumentNullException ("Continent cannot be null");

            Naam = name;
            Werelddeel = werelddeel;
        }

        public void UpdateDetails (string name, Werelddeel werelddeel)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");
            if (werelddeel is null)
                throw new ArgumentNullException ("Continent cannot be null");

            Naam = name;
            Werelddeel = werelddeel;
        }

        public void AddBestemming (Bestemming bestemming)
        {
            if (bestemming is null)
                throw new ArgumentNullException ("Destination cannot be null");
            if (bestemming.Land != null)
                throw new InvalidOperationException ("Destination does not belong to this land");

            _bestemmingen.Add (bestemming);
        }
    }
}
