using System;
using System.Collections.Generic;
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

        public DateOnly Vertrek
        {
            get; set;
        }

        public int AantalDagen
        {
            get; set;
        }

        public decimal PrijsPerPersoon
        {
            get; set;
        }

        public int AantalVolwassenen
        {
            get; set;
        }

        public int AantalKinderen
        {
            get; set;
        }

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
