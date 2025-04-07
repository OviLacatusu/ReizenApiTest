using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class BestemmingDTO
    {
        public string Code { get; set; } = null!;

        public string Plaats { get; set; } = null!;

        public LandDTO Land { get; set; }

        //public BestemmingDTO (string plaats, string code, int? id)
        //{
        //    Plaats = plaats;
        //    Code = code;
        //    Id = id;
        //}
    }
}
