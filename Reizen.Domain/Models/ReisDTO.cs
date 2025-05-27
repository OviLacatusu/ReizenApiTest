using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Domain.Models
{
    public class ReisDTO
    {
        public int? Id
        {
            get; set;
        }

        public string BestemmingsCode { get; set; } = null!;
        [Required (ErrorMessage = "Field is required")]
        public DateOnly Vertrek
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range(0,365)]
        public int AantalDagen
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range(0,1000000)]
        public decimal PrijsPerPersoon
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range (0,100)]
        public int AantalVolwassenen
        {
            get; set;
        }
        [Required (ErrorMessage = "Field is required")]
        [Range(0, 100)]
        public int AantalKinderen
        {
            get; set;
        }
        //[Required]
        public BestemmingDTO Bestemming { get; set; } = null;
        //public ReisDTO (DateOnly date, int aantalDagen, decimal prijsPP, int aantalVolwassenen, int aantalKinderen, string bestemmingCode, int? id)
        //{
        //    Vertrek = date;
        //    AantalDagen = aantalDagen;
        //    PrijsPerPersoon = prijsPP;
        //    AantalVolwassenen = aantalVolwassenen;
        //    AantalKinderen = aantalKinderen;
        //    BestemmingsCode = bestemmingCode;
        //    Id = id;
        //}
    }
   
}
