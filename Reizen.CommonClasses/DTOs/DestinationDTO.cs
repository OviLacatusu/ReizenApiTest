
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class DestinationDTO
    {
        [Required (ErrorMessage = "Field is required")]
        [StringLength (10)]
        public string Code { get; set; } = null!;
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string PlaceName { get; set; } = null!;

        public CountryDTO? Country
        {
            get; set;
        }

        //public DestinationDTO (string placename, string code, int? id)
        //{
        //    PlaceName = placename;
        //    Code = code;
        //    Id = id;
        //}
    }
}
