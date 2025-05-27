//using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Klant
    {
        public int Id { get; private set; }
        public string Voornaam { get; private set; }

        public string Familienaam { get; private set; }
        public string Adres { get; private set; }
        public Woonplaats Woonplaats { get; private set; }

        private readonly List<Boeking> _boekingen = new List<Boeking>();

        public IReadOnlyCollection<Boeking> Boekingen => _boekingen.AsReadOnly();
        private Klant() { }

        public Klant (string voornaam, string familienaam, string adres, Woonplaats woonplaats)
        {
            if (string.IsNullOrEmpty (voornaam))
                throw new ArgumentException ("First name cannot be empty or null");
            if (string.IsNullOrEmpty (familienaam))
                throw new ArgumentException ("Second name cannot be empty or null");
            if (string.IsNullOrEmpty (adres))
                throw new ArgumentNullException ("Address cannot be empty or null");
            if (woonplaats is null)
                throw new ArgumentNullException ("Woonplaats cannot be null");

            Voornaam = voornaam;
            Familienaam = familienaam;
            Adres = adres;
            Woonplaats = woonplaats;    
        }

        public void UpdateDetails (string voornaam, string familienaam, string adres, Woonplaats woonplaats)
        {
            if (string.IsNullOrEmpty (voornaam))
                throw new ArgumentException ("First name cannot be empty or null");
            if (string.IsNullOrEmpty (familienaam))
                throw new ArgumentException ("Second name cannot be empty or null");
            if (string.IsNullOrEmpty(adres))
                throw new ArgumentNullException ("Address cannot be empty or null");
            if (woonplaats is null)
                throw new ArgumentNullException ("Woonplaats cannot be null");

            Voornaam = voornaam;
            Familienaam = familienaam;
            Adres = adres;
            Woonplaats = woonplaats;
        }

        public void AddBoeking (Boeking boeking)
        {
            if (boeking is null)
                throw new ArgumentNullException ("Booking cannot be null");
            if (boeking.Klant != this)
                throw new InvalidOperationException ("Boeking does not belong to this client");

            _boekingen.Add (boeking);
        }
    }
}
