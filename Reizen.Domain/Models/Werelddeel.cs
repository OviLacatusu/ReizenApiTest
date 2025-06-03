using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Werelddeel
    {
        public int Id { get; private set; }
        public string Naam { get; private set; }
        private readonly List<Land> _landen = new List<Land> ();

        public ReadOnlyCollection<Land> Landen => _landen.AsReadOnly ();

        private Werelddeel ()
        {
        }
        public Werelddeel (string name)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");
            
            Naam = name;
        }
        public void AddLand (Land land)
        {
            if (land is null)
                throw new ArgumentNullException ("Land cannot be null");
            if (_landen.Where (l => l.Naam == land.Naam).Any ())
                throw new ArgumentException ("Land already added");
            if (land.Werelddeel != this)
                throw new InvalidOperationException ("Land does not belong to this continent");
            
            _landen.Add (land);
        }
        public void UpdateDetails (string name)
        {
            if (string.IsNullOrEmpty (name))
                throw new ArgumentException ("Name cannot be empty or null");

            Naam = name;

        }
    }
}
