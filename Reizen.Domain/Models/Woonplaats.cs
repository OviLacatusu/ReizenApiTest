using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Woonplaats
    {
        public int Id
        {
            get; private set;
        }
        public string Naam
        {
            get; private set;
        }
        public int Postcode
        {
            get; private set;
        }
        public IReadOnlyCollection<Klant> Klanten => _klanten.AsReadOnly ();

        private readonly List<Klant> _klanten = new ();

        // Private constructor for EF Core
        private Woonplaats ()
        {
        }
        public Woonplaats (string naam, int postcode)
        {
            if (string.IsNullOrEmpty (naam))
                throw new ArgumentException ("Naam cannot be empty", nameof (naam));
            if (postcode <= 0 || postcode > 99999)
                throw new ArgumentException ("Postcode must be between 1 and 99999", nameof (postcode));

            Naam = naam;
            Postcode = postcode;
        }

        public void UpdateDetails (string naam, int postcode)
        {
            if (string.IsNullOrEmpty (naam))
                throw new ArgumentException ("Naam cannot be empty", nameof (naam));
            if (postcode <= 0 || postcode > 9999)
                throw new ArgumentException ("Postcode must be between 1 and 9999", nameof (postcode));

            Naam = naam;
            Postcode = postcode;
        }

        public void AddClient (Klant klant)
        {
            if (klant == null)
                throw new ArgumentNullException (nameof (klant));

            if (klant.Woonplaats != this)
                throw new InvalidOperationException ("Klant does not belong to this woonplaats");

            _klanten.Add (klant);
        }
    }
}