using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public class DeleteKlant
    {
        public record DeleteKlantCommand (int klantId, ReizenContext context) : ICommand<Wrapper<int>>;

        public class DeleteKlantCommandHandler : ICommandHandler<DeleteKlantCommand, Wrapper<int>>
        {
            public async Task<Wrapper<int>> Execute (DeleteKlantCommand command)
            {
                Klant klant = new Klant { Id = command.klantId };
                command.context.Attach (klant);
                command.context.Klanten.Remove (klant);

                return new Wrapper<int>(await command.context.SaveChangesAsync ());
            }
        }     
    }
}
