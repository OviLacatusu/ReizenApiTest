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
        public record UpdateBoekingCommand(Boeking boeking, int id, ReizenContext context): ICommand<Result<Boeking>>;

        public class UpdateBoekingCommandHandler : ICommandHandler<UpdateBoekingCommand, Result<Boeking>>
        {
            public async Task<Result<Boeking>> Handle (UpdateBoekingCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingBoeking = await command.context.Boekingen.FindAsync (command.id);
                            if (existingBoeking == null)
                            {
                                return Result<Boeking>.Failure ($"Cannot find booking with ID");
                            }

                            existingBoeking.AantalKinderen = command.boeking.AantalKinderen;
                            existingBoeking.AantalVolwassenen = command.boeking.AantalVolwassenen;
                            existingBoeking.AnnulatieVerzekering = command.boeking.AnnulatieVerzekering;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<Boeking>.Success(command.boeking);
                            
                        }
                        catch 
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<Boeking>.Failure ($"Error updating booking: {ex.Message}");
                }
                
            }
        }
    }
}
