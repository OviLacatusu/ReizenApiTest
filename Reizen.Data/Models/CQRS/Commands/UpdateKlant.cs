using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateKlant
    {
        public record UpdateKlantCommand (Klant klantData, int klantId, ReizenContext context) : ICommand<Klant>;

        public class UpdateKlantCommandHandler : ICommandHandler<UpdateKlantCommand, Klant>
        {
            public async Task<Klant> Execute (UpdateKlantCommand command)
            {
                var klant = await command.context.Klanten.FindAsync (command.klantId);
                if (klant == null)
                    return null;
                klant.Voornaam = command.klantData.Voornaam;
                klant.Familienaam = command.klantData.Familienaam;
                klant.Adres = command.klantData.Adres;
                
                await command.context.SaveChangesAsync ();

                return klant;
            }
        }
        
    }
}
