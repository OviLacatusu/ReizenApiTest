using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class CountryDTO
    {
        public int? Id
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string Name { get; set; } = null!;
        public ContinentDTO? Continent { get; set; } = null!;

        //public CountryDTO(string name, int? id) 
        //{ 
        //    this.Name = name;
        //    this.Id = id;
        //}
    }
}
