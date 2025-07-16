//using Trips.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Client
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string FamilyName { get; private set; }
        public string Address { get; private set; }
        public Residence Residence { get; private set; }

        private readonly List<Booking> _bookings = new List<Booking>();

        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();
        private Client() { }

        public Client (string firstname, string familyname, string address, Residence residence)
        {
            if (string.IsNullOrEmpty (firstname))
                throw new ArgumentException ("First name cannot be empty or null");
            if (string.IsNullOrEmpty (familyname))
                throw new ArgumentException ("Second name cannot be empty or null");
            if (string.IsNullOrEmpty (address))
                throw new ArgumentNullException ("Address cannot be empty or null");
            if (residence is null)
                throw new ArgumentNullException ("Residence cannot be null");

            FirstName = firstname;
            FamilyName = familyname;
            Address = address;
            Residence = residence;    
        }

        public void UpdateDetails (string firstname, string familyname, string address, Residence residence)
        {
            if (string.IsNullOrEmpty (firstname))
                throw new ArgumentException ("First name cannot be empty or null");
            if (string.IsNullOrEmpty (familyname))
                throw new ArgumentException ("Second name cannot be empty or null");
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException ("Address cannot be empty or null");
            if (residence is null)
                throw new ArgumentNullException ("Residence cannot be null");

            FirstName = firstname;
            FamilyName = familyname;
            Address = address;
            Residence = residence;
        }

        public void AddBooking (Booking booking)
        {
            if (booking is null)
                throw new ArgumentNullException ("Booking cannot be null");
            if (booking.Client != this)
                throw new InvalidOperationException ("Booking does not belong to this client");

            _bookings.Add (booking);
        }

        public IEnumerable<Booking> GetActiveBookings ()
        {
            return _bookings.Where (b => b.Trip.DateOfDeparture > DateOnly.FromDateTime (DateTime.Today));
        }
    }
}
