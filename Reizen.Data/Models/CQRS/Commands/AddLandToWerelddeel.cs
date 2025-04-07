using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class AddLandToWerelddeel
    {
        public record AddLandToWerelddeelCommand(Land land, Werelddeel deel, ReizenContext context): ICommand<Wrapper<int>>;

        public class AddLandToWerelddeelCommandHandler : ICommandHandler<AddLandToWerelddeelCommand, Wrapper<int>>
        {
            public async Task<Wrapper<int>> Execute (AddLandToWerelddeelCommand command)
            {
                var deelw = await command.context.Werelddelen.FindAsync (command.deel);
                if (deelw?.Landen.Where (l => command.land.Naam == l.Naam).Count() == 0)
                {
                    deelw?.Landen.Add (command.land);
                }
                return new Wrapper<int>(await command.context.SaveChangesAsync ());
            }
        }
    }
}
