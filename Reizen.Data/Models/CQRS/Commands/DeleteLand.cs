using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteLand
    {
        public record DeleteLandCommand (int id, ReizenContext context) : ICommand<Land?>;

        public class DeleteLandCommandHandler (int id) : ICommandHandler<DeleteLandCommand, Land?>
        {
            public async Task<Land?> Execute (DeleteLandCommand command)
            {
                using (var transaction = await command.context.Database.BeginTransactionAsync ())
                {
                    Land? land = new Land { Id = command.id };
                    command.context.Attach (land);
                    command.context.Landen.Remove (land);

                    await transaction.CommitAsync ();
                    return land;
                }
            }
        }
    }
}
