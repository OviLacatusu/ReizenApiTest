using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Client.Models
{
    public class ListOfTripsModel
    {
        [MinLength (3, ErrorMessage = "Please enter at least 3 characters")]
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string? NameString { get; set; }
    }
}
