//using Trips.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Booking
    {
        public int Id { get; private set; }
        public Trip Trip { get; private set; }
        public Client Client { get; private set; }
        public DateOnly BookedOnDate { get; private set; }
        public int NumberOfAdults { get; private set; }
        public int NumberOfMinors { get; private set; }
        public bool AnnulatieVerzekering { get; private set; }

        private Booking() { }

        public Booking (DateOnly bookedOn, int numberOfAdults, int numberOfMinors, bool annulatieVerzekering, Trip trip, Client client)
        {
            if (bookedOn < DateOnly.FromDateTime (DateTime.Today))
                throw new ArgumentException ("Date of booking cannot be in the past");
            if (numberOfAdults <= 0)
                throw new ArgumentException ("Number of adults cannot be 0 or negative");
            if (numberOfMinors < 0)
                throw new ArgumentException ("Number of kids cannot be negative");
            if (trip is null)
                throw new ArgumentNullException ("Trip cannot be null");
            if (client is null)
                throw new ArgumentNullException ("Client cannot be null");

            BookedOnDate = bookedOn;
            NumberOfMinors = numberOfMinors;
            NumberOfAdults = numberOfAdults;
            AnnulatieVerzekering = annulatieVerzekering;
            Trip = trip;
            Client = client;
        }
        public void UpdateDetails (int numberOfAdults, int numberOfMinors, bool annulatieVerzekering, Trip trip)
        {
            if (numberOfAdults <= 0)
                throw new ArgumentException ("Number of adults cannot be 0 or negative");
            if (numberOfMinors < 0)
                throw new ArgumentException ("Number of kids cannot be negative");
            if (trip is null)
                throw new ArgumentNullException ("Trip cannot be null");

            NumberOfAdults = numberOfAdults;
            NumberOfMinors = numberOfMinors;
            AnnulatieVerzekering = annulatieVerzekering ;
            Trip = trip;
        }
        //TO DO: Calculating total price of booking
    }
}
