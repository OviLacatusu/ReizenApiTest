using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class TripDTO
    {
        public int? Id
        {
            get; set;
        }

        public string DestinationCode { get; set; } = null!;
        [Required (ErrorMessage = "Field is required")]
        public DateOnly DateOfDeparture
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 365)]
        public int NumberOfDays
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 1000000)]
        public decimal PricePerPerson
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100)]
        public int NumberOfAdults
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100)]
        public int NumberOfMinors
        {
            get; set;
        }
        //[Required]
        public DestinationDTO Destination { get; set; } = null;
        //public TripDTO (DateOnly date, int numberOfDays, decimal prijsPP, int numberOfAdults, int numberOfMinors, string DestinationCode, int? id)
        //{
        //    DateOfDeparture = date;
        //    NumberOfDays = numberOfDays;
        //    PricePerPerson = prijsPP;
        //    NumberOfAdults = numberOfAdults;
        //    NumberOfMinors = numberOfMinors;
        //    DestinationCode = DestinationCode;
        //    Id = id;
        //}
    }

}
