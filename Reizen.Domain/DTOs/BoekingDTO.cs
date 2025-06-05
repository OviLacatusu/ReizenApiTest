using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.DTOs
{
    public class BoekingDTO
    {
        public int Id
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, int.MaxValue)]
        public int Klantid
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, int.MaxValue)]
        public int Reisid
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        public DateOnly GeboektOp
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100)]
        public int? AantalVolwassenen
        {
            get; 
            set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100)]
        public int? AantalKinderen
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        public bool AnnulatieVerzekering
        {
            get; set;
        }
    }
}
