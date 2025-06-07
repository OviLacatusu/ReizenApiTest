using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateBoeking
    {
        public record UpdateBoekingCommand(BoekingDAL boeking, int id, ReizenContext context): ICommand<Result<BoekingDAL>>;

        public class UpdateBoekingCommandHandler : ICommandHandler<UpdateBoekingCommand, Result<BoekingDAL>>
        {
            public async Task<Result<BoekingDAL>> Handle (UpdateBoekingCommand command)
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
                                return Result<BoekingDAL>.Failure ($"Cannot find booking with ID");
                            }

                            existingBoeking.AantalKinderen = command.boeking.AantalKinderen;
                            existingBoeking.AantalVolwassenen = command.boeking.AantalVolwassenen;
                            existingBoeking.AnnulatieVerzekering = command.boeking.AnnulatieVerzekering;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<BoekingDAL>.Success(command.boeking);
                            
                        }
                        catch 
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<BoekingDAL>.Failure ($"Error updating booking: {ex.Message}");
                }
                
            }
        }
    }
}
