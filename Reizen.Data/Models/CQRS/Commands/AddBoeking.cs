using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddBoeking
    {
        public record AddBoekingCommand(Boeking boeking, ReizenContext context) : ICommand<Boeking?>;

        public class AddBoekingCommandHandler : ICommandHandler<AddBoekingCommand, Boeking?>
        {
            public async Task<Boeking?> Execute (AddBoekingCommand command)
            {
                using (var transaction = command.context.Database.BeginTransaction ())
                {
                    var boeking = await command.context.Boekingen.Where (b => b.Klantid == command.boeking.Klantid && b.Reisid == command.boeking.Klantid).ToListAsync();
                    if (!boeking.Any ())
                    {
                        var result = await command.context.Boekingen.AddAsync (command.boeking);
                        transaction.Commit ();
                        return command.boeking;
                    }
                }
                return null;
                
            }
        }
    }
}
