
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateLand
    {
        public record UpdateLandCommand (int id, Land landData, ReizenContext context ) : ICommand<Land?>;

        public class UpdateClassCommandHandler : ICommandHandler<UpdateLandCommand, Land?>
        {
            public async Task<Land?> Execute (UpdateLandCommand command)
            {
                using (var transaction = await command.context.Database.BeginTransactionAsync ())
                {
                    var land = await command.context.Landen.FindAsync (command.id);
                    if (land != null)
                    {
                        land.Werelddeel = command.landData.Werelddeel;
                        land.Naam = command.landData.Naam;
                        land.Bestemmingen = land.Bestemmingen;
                    }
                    transaction.Commit ();
                    return land;
                }
            }
        }
    }
}
