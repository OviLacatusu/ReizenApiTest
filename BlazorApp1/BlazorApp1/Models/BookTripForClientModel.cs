using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models
{
    public class BookTripForClientModel
    {
        [Required (ErrorMessage = "Field is required")]
        [Range (1, 100, ErrorMessage = "Value should be greater than {1} and lower than {2}")]
        public int? NumberOfAdults
        {
            get;
            set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0, 100, ErrorMessage = "Value should be greater than {1} and lower than {2}")]
        public int? NumberOfMinors
        {
            get; set;
        }
        public bool HasCancellationInsurance
        {
            get; set;
        }
    }
}
