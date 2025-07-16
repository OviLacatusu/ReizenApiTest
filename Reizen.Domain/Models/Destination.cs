
using Reizen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Destination
    {
        public string PlaceName { get; private set; }

        public string Code { get; private set; }

        public Country Country { get; private set; }

        private readonly List<Trip> _trips = new List<Trip> ();

        public ReadOnlyCollection<Trip> Trips => _trips.AsReadOnly ();

        private Destination ()
        {
        }

        public Destination (string name, string code, Country country)
        {
            if(String.IsNullOrEmpty(name)) 
                throw new ArgumentNullException ("Name cannot be empty or null");
            if (String.IsNullOrEmpty (code))
                throw new ArgumentNullException ("Code cannot be empty or null");
            if (country is null)
                throw new ArgumentNullException ("Country cannot be null");
            
            Code = code;
            Country = country;
            PlaceName = name;
        }

        public void UpdateDetails (string name, Country country)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException ("Name cannot be empty or null");
            if (country is null)
                throw new ArgumentNullException ("Country cannot be null");

            PlaceName = name;
            Country = country;
        }
        public void AddTrip (Trip trip)
        {
            if (trip is null)
                throw new ArgumentNullException ("Trip cannot be null");
            if (trip.Destination.Code != Code)
                throw new InvalidOperationException ("Trip has an invalid destination code");

            _trips.Add (trip);
        }
        public IEnumerable<Trip> GetFutureTrips ()
        {
            return _trips.Where (r => r.DateOfDeparture > DateOnly.FromDateTime (DateTime.Today)).ToList().AsReadOnly();
        }
    }
}
