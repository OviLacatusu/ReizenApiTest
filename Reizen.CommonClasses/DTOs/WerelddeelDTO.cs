using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class WerelddeelDTO
    {
        public int? Id
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string Naam { get; set; } = null!;

        //public WerelddeelDTO(string naam, int? id) 
        //{ 
        //    Naam = naam;
        //    Id = id;
        //}
    }
}
