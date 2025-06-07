using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteBoeking
    {
        public record DeleteBoekingCommand(int id, ReizenContext context) : ICommand<Result<BoekingDAL>>;

        public class DeleteBoekingCommandHandler : ICommandHandler<DeleteBoekingCommand, Result<BoekingDAL>>
        {
            public async Task<Result<BoekingDAL>> Handle (DeleteBoekingCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            if (command.id < 0)
                                return Result<BoekingDAL>.Failure ($"Id of booking cannot be negative");
                            var existingBoeking = command.context.Boekingen.FirstOrDefault (el => el.Id == command.id);
                            if (existingBoeking == null)
                            {
                                return Result<BoekingDAL>.Failure ($"Boeking does not exist");
                            }
                            
                            var boeking = new BoekingDAL { Id = command.id };

                            command.context.Attach (boeking);
                            command.context.Boekingen.Remove (boeking);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<BoekingDAL>.Success(boeking);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Result<BoekingDAL>.Failure ($"Error deleting booking: {ex.Message}");
                }
            }
        }
    }
}
