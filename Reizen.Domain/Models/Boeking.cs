//using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Boeking
    {
        public int Id { get; private set; }
        public Reis Reis { get; private set; }
        public Klant Klant { get; private set; }
        public DateOnly GeboektOp { get; private set; }
        public int AantalVolwassenen { get; private set; }
        public int AantalKinderen { get; private set; }
        public bool AnnulatieVerzekering { get; private set; }

        private Boeking() { }

        public Boeking (DateOnly bookedOn, int aantalVolwassenen, int aantalKinderen, bool annulatieVerzekering, Reis reis, Klant klant)
        {
            if (bookedOn < DateOnly.FromDateTime (DateTime.Today))
                throw new ArgumentException ("Date of booking cannot be in the past");
            if (aantalVolwassenen <= 0)
                throw new ArgumentException ("Number of adults cannot be 0 or negative");
            if (aantalKinderen < 0)
                throw new ArgumentException ("Number of kids cannot be negative");
            if (reis is null)
                throw new ArgumentNullException ("Trip cannot be null");
            if (klant is null)
                throw new ArgumentNullException ("Client cannot be null");

            GeboektOp = bookedOn;
            AantalKinderen = aantalKinderen;
            AantalVolwassenen = aantalVolwassenen;
            AnnulatieVerzekering = annulatieVerzekering;
            Reis = reis;
            Klant = klant;
        }
        public void UpdateDetails (int aantalVolwassenen, int aantalKinderen, bool annulatieVerzekering, Reis reis)
        {
            if (aantalVolwassenen <= 0)
                throw new ArgumentException ("Number of adults cannot be 0 or negative");
            if (aantalKinderen < 0)
                throw new ArgumentException ("Number of kids cannot be negative");
            if (reis is null)
                throw new ArgumentNullException ("Trip cannot be null");

            AantalVolwassenen = aantalVolwassenen;
            AantalKinderen = aantalKinderen;
            AnnulatieVerzekering = annulatieVerzekering ;
            Reis = reis;
        }
    }
}
