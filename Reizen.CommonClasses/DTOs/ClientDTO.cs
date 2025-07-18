
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.CommonClasses.DTOs
{
    public class ClientDTO
    {
        public int? Id
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string? FamilyName { get; set; }
        [Required (ErrorMessage = "Field is required")]
        [StringLength (100)]
        public string? FirstName { get; set; }

        public string? Address { get; set; }
        public ResidenceDTO? Residence { get; set; }

        //private ClientDTO (string firstname, string familyname, string address, int? id)
        //{
        //    this.FirstName = firstname;
        //    this.FamilyName = familyname;
        //    this.Address = address;
        //    this.Id = id;
        //}

        //public static Client (Data.Models.Client klant)
        //{

        //}
    }
}
