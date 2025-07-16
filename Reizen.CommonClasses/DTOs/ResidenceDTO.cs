using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class ResidenceDTO
    {
        public int? Id
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [StringLength (10)]
        public int PostalCode
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string Name { get; set; } = null!;

        //public ResidenceDTO(string name, int postalcode, int? id) 
        //{
        //    Name = name;
        //    PostalCode = postalcode;
        //    Id = id;
        //}
    }
}
