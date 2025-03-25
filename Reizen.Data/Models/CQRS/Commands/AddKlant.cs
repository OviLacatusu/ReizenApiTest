using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class AddKlant
    {
        public record AddKlantCommand(Klant klant, ReizenContext context) : ICommand<int>;

        public class AddKlantCommandHandler : ICommandHandler<AddKlantCommand, int>
        {
            public async Task<int> Execute (AddKlantCommand command)
            {
                await command.context.Klanten.AddAsync (command.klant);
                return command.context.SaveChangesAsync ().Id;
            }
        }
    }
}
