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
        public record DeleteKlantCommand (int klantId, ReizenContext context) : ICommand<int>;

        public class DeleteKlantCommandHandler : ICommandHandler<DeleteKlantCommand, int>
        {
            public async Task<int> Execute (DeleteKlantCommand command)
            {
                Klant klant = new Klant { Id = command.klantId };
                command.context.Attach (klant);
                command.context.Klanten.Remove (klant);

                return command.context.SaveChangesAsync ().Id;
            }
        }

        
    }
}
