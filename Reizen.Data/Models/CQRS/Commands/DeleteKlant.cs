using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteKlant
    {
        public record DeleteKlantCommand (int klantId, ReizenContext context) : ICommand<Klant?>;

        public class DeleteKlantCommandHandler : ICommandHandler<DeleteKlantCommand, Klant?>
        {
            public async Task<Klant?> Execute (DeleteKlantCommand command)
            {
                using var transaction = await command.context.Database.BeginTransactionAsync ();
                {

                    Klant klant = new Klant { Id = command.klantId };
                    command.context.Attach (klant);
                    command.context.Klanten.Remove (klant);

                    transaction.Commit ();

                    return klant;
                }
            }
        }     
    }
}
