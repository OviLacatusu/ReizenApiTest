using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Continent
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        private readonly List<Country> _Countries = new List<Country> ();

        public ReadOnlyCollection<Country> Countries => _Countries.AsReadOnly ();

        private Continent ()
        {
        }
        public Continent (string name)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");
            
            Name = name;
        }
        public void AddCountry (Country Country)
        {
            if (Country is null)
                throw new ArgumentNullException ("Country cannot be null");
            if (_Countries.Where (l => l.Name == Country.Name).Any ())
                throw new ArgumentException ("Country already added");
            if (Country.Continent != this)
                throw new InvalidOperationException ("Country does not belong to this continent");
            
            _Countries.Add (Country);
        }
        public void UpdateDetails (string name)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");

            Name = name;

        }
    }
}
