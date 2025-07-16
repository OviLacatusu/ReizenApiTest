using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class Residence
    {
        public int Id
        {
            get; private set;
        }
        public string Name
        {
            get; private set;
        }
        public int PostalCode
        {
            get; private set;
        }
        public IReadOnlyCollection<Client> Clients => _clients.AsReadOnly ();

        private readonly List<Client> _clients = new ();

        // Private constructor for EF Core
        private Residence ()
        {
        }
        public Residence (string nameOfPlace, int postalCode)
        {
            if (string.IsNullOrEmpty (nameOfPlace))
                throw new ArgumentException ("Name cannot be empty", nameof (nameOfPlace));
            if (postalCode <= 0 || postalCode > 99999)
                throw new ArgumentException ("PostalCode must be between 1 and 99999", nameof (postalCode));

            Name = nameOfPlace;
            PostalCode = postalCode;
        }

        public void UpdateDetails (string nameOfPlace, int postalCode)
        {
            if (string.IsNullOrEmpty (nameOfPlace))
                throw new ArgumentException ("Name cannot be empty", nameof (nameOfPlace));
            if (postalCode <= 0 || postalCode > 9999)
                throw new ArgumentException ("PostalCode must be between 1 and 9999", nameof (postalCode));

            Name = nameOfPlace;
            PostalCode = postalCode;
        }

        public void AddClient (Client client)
        {
            if (client == null)
                throw new ArgumentNullException (nameof (client));

            if (client.Residence != this)
                throw new InvalidOperationException ("Client does not belong to this residence");

            _clients.Add (client);
        }
    }
}