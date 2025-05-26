using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class BestemmingDTO
    {
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        public string Plaats { get; set; } = null!;
        [Required]
        public LandDTO Land { get; set; }

        //public BestemmingDTO (string plaats, string code, int? id)
        //{
        //    Plaats = plaats;
        //    Code = code;
        //    Id = id;
        //}
    }
}
