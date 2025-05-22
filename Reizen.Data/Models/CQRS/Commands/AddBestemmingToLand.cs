using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddBestemmingToLand
    {
        public record AddBestemmingToLandCommand (Bestemming bestemming, Land land, ReizenContext context) : ICommand<Bestemming?>;

        public class AddBestemmingToLandCommandHandler : ICommandHandler<AddBestemmingToLandCommand, Bestemming?>
        {
            public async Task<Bestemming?> Execute (AddBestemmingToLandCommand command)
            {
                using var transaction = await command.context.Database.BeginTransactionAsync ();
                {
                    var land = await command.context.Landen.FindAsync (command.land);
                    Bestemming bestemming = null;
                    if (land?.Bestemmingen.Where (el => el.Plaats == command.bestemming.Plaats).Count () == 0)
                    {
                        land.Bestemmingen.Add (command.bestemming);
                    }
                    await transaction.CommitAsync ();
                    return command.bestemming;
                }
            }
        }
    }
}
