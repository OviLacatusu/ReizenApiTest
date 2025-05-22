using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteBestemming
    {
        public record DeleteBestemmingCommand(string code, ReizenContext context) : ICommand<Bestemming?>;

        public class DeleteBestemmingCommandHandler : ICommandHandler<DeleteBestemmingCommand, Bestemming?>
        {
            public async Task<Bestemming?> Execute (DeleteBestemmingCommand command)
            {
                using (var transaction = await command.context.Database.BeginTransactionAsync ())
                {
                    var bestemming = new Bestemming { Code = command.code };
                    command.context.Attach (bestemming);
                    command.context.Bestemmingen.Remove (bestemming);

                    transaction.Commit ();
                    
                    return bestemming;
                }
            }
        }
    }
}
