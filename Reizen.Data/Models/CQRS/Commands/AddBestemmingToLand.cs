using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class AddBestemmingToLand
    {
        public record AddBestemmingToLandCommand (Bestemming bestemming, Land land, ReizenContext context) : ICommand<int>;

        public class AddBestemmingToLandCommandHandler : ICommandHandler<AddBestemmingToLandCommand, int>
        {
            public async Task<int> Execute (AddBestemmingToLandCommand command)
            {
                var land = await command.context.Landen.FindAsync(command.land);
                if (land?.Bestemmingen.Where (el => el.Plaats == command.bestemming.Plaats).Count() == 0)
                {
                    land.Bestemmingen.Add (command.bestemming);
                }
                return await command.context.SaveChangesAsync ();
            }
        }
    }
}
