using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddLand
    {
        public record AddLandCommand (Land land, ReizenContext context) : ICommand<Land>;

        public class AddLandCommandHandler : ICommandHandler<AddLandCommand, Land?> 
        {
            public async Task<Land?> Execute (AddLandCommand command)
            {
                var result = await command.context.Landen.FindAsync (command.land.Naam);
                if (result == null)
                {
                    await command.context.Landen.AddAsync (command.land);
                    await command.context.SaveChangesAsync ();
                }
                return command.land;
            }
        }
    }
}
