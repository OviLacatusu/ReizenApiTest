using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class WoonplaatsDTO
    {
        public int? Id { get; set; }

        public int Postcode { get; set; }

        public string Naam { get; set; } = null!;

        //public WoonplaatsDTO(string naam, int postcode, int? id) 
        //{
        //    Naam = naam;
        //    Postcode = postcode;
        //    Id = id;
        //}
    }
}
