using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Reis
    {
        public int Id { get; private set; }

        public int AantalDagen { get; private set; }
        public decimal PrijsPerPersoon { get; private set; }

        public Bestemming Bestemming { get; private set; }
        public DateOnly VertrekDatum { get; private set; }

        private readonly List<Boeking> _boekingen = new List<Boeking>();
        public ReadOnlyCollection<Boeking> Boekingen => _boekingen.AsReadOnly ();

        private Reis() { }

        public Reis (DateOnly vertrek, int aantalDagen, decimal prijsPerPersoon, Bestemming bestemming)
        {
            if (vertrek < DateOnly.FromDateTime (DateTime.Today))
                throw new ArgumentException ("Date of trip cannot be null or in the past");
            if (aantalDagen <= 0)
                throw new ArgumentException ("Number of days cannot be 0 or negative");
            if (prijsPerPersoon < 0)
                throw new ArgumentException ("Price cannot be negative");
            if (bestemming is null)
                throw new ArgumentNullException ("Destination cannot be null");

            VertrekDatum = vertrek;
            Bestemming = bestemming;
            AantalDagen = aantalDagen;
            PrijsPerPersoon = prijsPerPersoon;
        }

        public void UpdateDetails (DateOnly vertrek, int aantalDagen, decimal prijsPerPersoon, int aantalVolwassenen, int aantalKinderen)
        {
            if (vertrek < DateOnly.FromDateTime (DateTime.Today))
                throw new ArgumentException ("Date of trip cannot be null or in the past");
            if (aantalDagen <= 0)
                throw new ArgumentException ("Number of days cannot be 0 or negative");
            if (prijsPerPersoon < 0)
                throw new ArgumentException ("Price cannot be negative");
            if (aantalVolwassenen <= 0)
                throw new ArgumentException ("Number of grownups cannot be 0 or negative");
            if (aantalKinderen < 0)
                throw new ArgumentException ("Number of kids cannot be negative");

            VertrekDatum = vertrek;
            AantalDagen = aantalDagen;
            PrijsPerPersoon = prijsPerPersoon;
        }

        public void AddBoeking (Boeking boeking)
        {
            if (boeking is null)
                throw new ArgumentNullException ("Boeking cannot be null");
            if (boeking.Reis != this)
                throw new InvalidOperationException ("Boeking doesn't belong to this trip");

            _boekingen.Add (boeking);
        }

        public decimal CalculatePrice ()
        {
            return _boekingen.Select (b => new { b.AantalVolwassenen, b.AantalKinderen }).Sum (b => (b.AantalVolwassenen+b.AantalKinderen)*PrijsPerPersoon);
        }

    }
}
