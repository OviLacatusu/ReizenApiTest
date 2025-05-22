using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddKlant
    {
        public record AddKlantCommand(Klant klant, ReizenContext context) : ICommand<Klant?>;

        public class AddKlantCommandHandler : ICommandHandler<AddKlantCommand, Klant?>
        {
            public async Task<Klant?> Execute (AddKlantCommand command)
            {
                var result = await command.context.Klanten.AddAsync (command.klant);
                await command.context.SaveChangesAsync ();
                
                return result.Entity;
            }
        }
    }
}
