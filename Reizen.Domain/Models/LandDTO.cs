using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class LandDTO
    {
        public int? Id { get; set; }

        public string Naam { get; set; } = null!;

        public WerelddeelDTO Werelddeel { get; set;} = null!;

        //public LandDTO(string naam, int? id) 
        //{ 
        //    this.Naam = naam;
        //    this.Id = id;
        //}
    }
}
