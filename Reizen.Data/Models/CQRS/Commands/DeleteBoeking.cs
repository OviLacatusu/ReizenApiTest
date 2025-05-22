using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteBoeking
    {
        public record DeleteBoekingCommand(int id, ReizenContext context) : ICommand<Boeking?>;

        public class DeleteBoekingCommandHandler : ICommandHandler<DeleteBoekingCommand, Boeking?>
        {
            public async Task<Boeking?> Execute (DeleteBoekingCommand command)
            {
                using (var transaction = await command.context.Database.BeginTransactionAsync ())
                {
                    var existingBoeking = command.context.Boekingen.FirstOrDefault (el => el.Id == command.id);
                    if (existingBoeking != null)
                    {
                        var boeking = new Boeking { Id = command.id };
                        command.context.Attach (boeking);
                        command.context.Boekingen.Remove (boeking);
                        await transaction.CommitAsync ();

                        return boeking;
                    }
                }
                return null;
            }
        }
    }
}
