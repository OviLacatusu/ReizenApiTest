using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class WoonplaatsDTO
    {
        public int? Id { get; set; }
        [Required]
        public int Postcode { get; set; }
        [Required]
        public string Naam { get; set; } = null!;

        //public WoonplaatsDTO(string naam, int postcode, int? id) 
        //{
        //    Naam = naam;
        //    Postcode = postcode;
        //    Id = id;
        //}
    }
}
