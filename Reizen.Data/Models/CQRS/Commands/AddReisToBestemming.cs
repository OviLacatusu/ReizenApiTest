using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddReisToBestemming
    {
        public record AddReisToBestemmingCommand(Reis reis, Bestemming bestemming, ReizenContext context):ICommand<Reis?>;

        public class AddReisToBestemmingCommandHandler : ICommandHandler<AddReisToBestemmingCommand, Reis?>
        {
            public async Task<Reis?> Execute (AddReisToBestemmingCommand command)
            {
                using var transaction = await command.context.Database.BeginTransactionAsync ();
                {
                    var bestemming = await command.context.Bestemmingen.FindAsync (command.bestemming);
                    
                    bestemming?.Reizen.Add (command.reis);

                    await transaction.CommitAsync ();

                    return command.reis;
                }
            }
        }
    }
}
