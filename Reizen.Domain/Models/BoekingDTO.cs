using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class BoekingDTO
    {
        public int Id
        {
            get; set;
        }
        [Required]
        public int Klantid
        {
            get; set;
        }
        [Required]
        
        public int Reisid
        {
            get; set;
        }
        [Required]
        public DateOnly GeboektOp
        {
            get; set;
        }
        [Required]
        public int? AantalVolwassenen
        {
            get; set;
        }
        [Required]
        public int? AantalKinderen
        {
            get; set;
        }
        [Required]
        public bool AnnulatieVerzekering
        {
            get; set;
        }
    }
}
