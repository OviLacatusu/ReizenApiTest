using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class AddReisToBestemming
    {
        public record AddReisToBestemmingCommand(Reis reis, Bestemming bestemming, ReizenContext context):ICommand<int>;

        public class AddReisToBestemmingCommandHandler : ICommandHandler<AddReisToBestemmingCommand, int>
        {
            public async Task<int> Execute (AddReisToBestemmingCommand command)
            {
                var bestemming = await command.context.Bestemmingen.FindAsync (command.bestemming);
                bestemming?.Reizen.Add (command.reis);
                return await command.context.SaveChangesAsync ();
            }
        }
    }
}
