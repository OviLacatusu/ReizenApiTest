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
        public record AddReisToBestemmingCommand(Reis reis, Bestemming bestemming, ReizenContext context):ICommand<Wrapper<int>>;

        public class AddReisToBestemmingCommandHandler : ICommandHandler<AddReisToBestemmingCommand, Wrapper<int>>
        {
            public async Task<Wrapper<int>> Execute (AddReisToBestemmingCommand command)
            {
                var bestemming = await command.context.Bestemmingen.FindAsync (command.bestemming);
                bestemming?.Reizen.Add (command.reis);
                return new Wrapper<int>(await command.context.SaveChangesAsync ());
            }
        }
    }
}
