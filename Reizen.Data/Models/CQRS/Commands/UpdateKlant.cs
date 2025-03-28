using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class UpdateKlant
    {
        public record UpdateKlantCommand (Klant klantData, int klantId, ReizenContext context) : ICommand<int>;

        public class UpdateKlantCommandHandler : ICommandHandler<UpdateKlantCommand, int>
        {
            public async Task<int> Execute (UpdateKlantCommand command)
            {
                var klant = await command.context.Klanten.FindAsync (command.klantId);
                
                klant.Voornaam = command.klantData.Voornaam;
                klant.Familienaam = command.klantData.Familienaam;
                klant.Adres = command.klantData.Adres;
                
                return command.context.SaveChangesAsync ().Id;
            }
        }
        
    }
}
