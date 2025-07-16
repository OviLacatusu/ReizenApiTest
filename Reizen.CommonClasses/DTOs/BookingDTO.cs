using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class BookingDTO
    {
        public int Id
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, int.MaxValue)]
        public int ClientId
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, int.MaxValue)]
        public int TripId
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        public DateOnly BookedOnDate
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100)]
        public int? NumberOfAdults
        {
            get; 
            set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100)]
        public int? NumberOfMinors
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
