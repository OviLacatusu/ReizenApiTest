using Reizen.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReizenApi.Models
{
    public class KlantDTO
    {
        public int? Id
        {
            get; set;
        }

        public string Familienaam { get; set; } = null!;

        public string Voornaam { get; set; } = null!;

        public string Adres { get; set; } = null!;

        private KlantDTO (string voornaam, string familienaam, string adres, int? id)
        {
            this.Voornaam = voornaam;
            this.Familienaam = familienaam;
            this.Adres = adres;
            this.Id = id;
        }

        //public static Klant (Data.Models.Klant klant)
        //{

        //}
    }
}
