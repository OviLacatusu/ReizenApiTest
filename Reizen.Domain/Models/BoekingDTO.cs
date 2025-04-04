using System;
using System.Collections.Generic;
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

        public int Klantid
        {
            get; set;
        }

        public int Reisid
        {
            get; set;
        }

        public DateOnly GeboektOp
        {
            get; set;
        }

        public int? AantalVolwassenen
        {
            get; set;
        }

        public int? AantalKinderen
        {
            get; set;
        }

        public bool AnnulatieVerzekering
        {
            get; set;
        }
    }
}
