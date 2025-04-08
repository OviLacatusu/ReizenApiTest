using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class AddBestemming
    {
        public record AddBestemmingCommand(ReizenContext context, Bestemming bestemming): ICommand<Bestemming>;

        public class AddBestemmingCommandHandler () : ICommandHandler<AddBestemmingCommand, Bestemming>
        {
            public async Task<Bestemming> Execute (AddBestemmingCommand command)
            {
                var result = await command.context.Bestemmingen.AddAsync (command.bestemming);
                await command.context.SaveChangesAsync();

                return result.Entity;
            }
        }
    }
}
