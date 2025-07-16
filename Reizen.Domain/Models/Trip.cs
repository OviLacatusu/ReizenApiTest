//using Trips.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reizen.Domain.Models;

namespace Reizen.Domain.Models
{
    public class Trip
    {
        public int Id { get; private set; }

        public int NumberOfDays { get; private set; }
        public decimal PricePerPerson { get; private set; }
        public int NumberOfAvailableSpots { get; private set; }
        public Destination Destination { get; private set; }
        public DateOnly DateOfDeparture { get; private set; }

        private readonly List<Booking> _bookings = new List<Booking>();
        public ReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly ();

        private Trip() { }

        public Trip (DateOnly dateOfDeparture, int numberOfDays, int numberOfSpots, decimal pricePerPerson, Destination destination)
        {
            if (dateOfDeparture < DateOnly.FromDateTime (DateTime.Today))
                throw new ArgumentException ("Date of trip cannot be null or in the past");
            if (numberOfDays <= 0)
                throw new ArgumentException ("Number of days cannot be 0 or negative");
            if (pricePerPerson < 0)
                throw new ArgumentException ("Price cannot be negative");
            if (destination is null)
                throw new ArgumentNullException ("Destination cannot be null");
            if (numberOfSpots <= 0)
                throw new ArgumentException ("Number of available places must be greater than 0");

            DateOfDeparture = dateOfDeparture;
            Destination = destination;
            NumberOfDays = numberOfDays;
            NumberOfAvailableSpots = numberOfSpots;
            PricePerPerson = pricePerPerson;
        }

        public void UpdateDetails (DateOnly dateOfDeparture, int numberOfDays, decimal pricePerPerson, int numberOfSpots)
        {
            if (dateOfDeparture < DateOnly.FromDateTime (DateTime.Today))
                throw new ArgumentException ("Date of trip cannot be null or in the past");
            if (numberOfDays <= 0)
                throw new ArgumentException ("Number of days cannot be 0 or negative");
            if (pricePerPerson <= 0)
                throw new ArgumentException ("Price cannot be negative");
            if (numberOfSpots <= 0)
                throw new ArgumentException ("Number of grownups cannot be 0 or negative");

            DateOfDeparture = dateOfDeparture;
            NumberOfDays = numberOfDays;
            NumberOfAvailableSpots = numberOfSpots ;
            PricePerPerson = pricePerPerson;
        }

        public void AddBooking (Booking booking)
        {
            if (booking is null)
                throw new ArgumentNullException ("Booking cannot be null");
            if (booking.Trip != this)
                throw new InvalidOperationException ("Booking doesn't belong to this trip");
            if (Bookings.Sum (b => b.NumberOfMinors + b.NumberOfAdults) + booking.NumberOfAdults + booking.NumberOfMinors > NumberOfAvailableSpots)
                throw new NotSupportedException ("Number of places is exceeded");

            _bookings.Add (booking);
        }
        // Prices are the same for adults and kids
        public decimal CalculatePrice ()
        {
            return Bookings.Select (b => new { b.NumberOfAdults, b.NumberOfMinors }).Sum (b => (b.NumberOfAdults+b.NumberOfMinors)*PricePerPerson);
        }
        public int AantalBeschikbarePlaceNameen ()
        {
            return NumberOfAvailableSpots - Bookings.Sum (b => b.NumberOfAdults + b.NumberOfMinors);
        }

    }
}
