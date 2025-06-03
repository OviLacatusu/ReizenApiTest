//using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Bestemming
    {
        public string Plaats { get; private set; }

        public string Code { get; private set; }

        public Land Land { get; private set; }

        private readonly List<Reis> _reizen = new List<Reis> ();

        public ReadOnlyCollection<Reis> Reizen => _reizen.AsReadOnly ();

        private Bestemming ()
        {
        }

        public Bestemming (string name, string code, Land land)
        {
            if(String.IsNullOrEmpty(name)) 
                throw new ArgumentNullException ("Name cannot be empty or null");
            if (String.IsNullOrEmpty (code))
                throw new ArgumentNullException ("Code cannot be empty or null");
            if (land is null)
                throw new ArgumentNullException ("Land cannot be null");
            
            Code = code;
            Land = land;
            Plaats = name;
        }

        public void UpdateDetails (string name, Land land)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException ("Name cannot be empty or null");
            if (land is null)
                throw new ArgumentNullException ("Land cannot be null");

            Plaats = name;
            Land = land;
        }
        public void AddReis (Reis reis)
        {
            if (reis is null)
                throw new ArgumentNullException ("Reis cannot be null");
            if (reis.Bestemming.Code != Code)
                throw new InvalidOperationException ("Reis has an invalid destination code");

            _reizen.Add (reis);
        }

        public IEnumerable<Reis> GetFutureTrips ()
        {
            return _reizen.Where (r => r.Vertrek > DateOnly.FromDateTime (DateTime.Today)).ToList().AsReadOnly();
        }
    }
}
