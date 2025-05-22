using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateBoeking
    {
        public record UpdateBoekingCommand(Boeking boeking, int id, ReizenContext context): ICommand<Boeking?>;

        public class UpdateBoekingCommandHandler : ICommandHandler<UpdateBoekingCommand, Boeking?>
        {
            public async Task<Boeking?> Execute (UpdateBoekingCommand command)
            {
                using (var transaction = command.context.Database.BeginTransaction ())
                {
                    var existingBoeking = await command.context.Boekingen.FindAsync (command.id);
                    if (existingBoeking != null)
                    {
                        existingBoeking.AantalKinderen = command.boeking.AantalKinderen;
                        existingBoeking.AantalVolwassenen = command.boeking.AantalVolwassenen;
                        existingBoeking.Reisid = command.boeking.Reisid;
                        existingBoeking.AnnulatieVerzekering = command.boeking.AnnulatieVerzekering;

                        transaction.Commit ();
                        return command.boeking;
                    }
                    return null;
                }
                
            }
        }
    }
}
