using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteReis
    {
        public record DeleteReisCommand(int id, ReizenContext context):ICommand<Reis?>;

        public class DeleteReisCommandHandler : ICommandHandler<DeleteReisCommand, Reis?>
        {
            public async Task<Reis?> Execute (DeleteReisCommand command)
            {
                using (var transaction = await command.context.Database.BeginTransactionAsync ())
                {
                    var reis = new Reis { Id = command.id };
                    command.context.Attach (reis);
                    command.context.Reizen.Remove (reis);

                    await transaction.CommitAsync ();
                    return reis;
                }
            }
        }
    }
}
