using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class WerelddeelDTO
    {
        public int? Id { get; set; }

        public string Naam { get; set; } = null!;

        //public WerelddeelDTO(string naam, int? id) 
        //{ 
        //    Naam = naam;
        //    Id = id;
        //}
    }
}
