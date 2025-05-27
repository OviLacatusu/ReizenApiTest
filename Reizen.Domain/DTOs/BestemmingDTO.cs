
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.DTOs
{
    public class BestemmingDTO
    {
        [Required (ErrorMessage = "Field is required")]
        [StringLength (10)]
        public string Code { get; set; } = null!;
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string Plaats { get; set; } = null!;

        public LandDTO? Land
        {
            get; set;
        }

        //public BestemmingDTO (string plaats, string code, int? id)
        //{
        //    Plaats = plaats;
        //    Code = code;
        //    Id = id;
        //}
    }
}
