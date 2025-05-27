using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteBoeking
    {
        public record DeleteBoekingCommand(int id, ReizenContext context) : ICommand<Result<Boeking>>;

        public class DeleteBoekingCommandHandler : ICommandHandler<DeleteBoekingCommand, Result<Boeking>>
        {
            public async Task<Result<Boeking>> Handle (DeleteBoekingCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            if (command.id < 0)
                                return Result<Boeking>.Failure ($"Id of booking cannot be negative");
                            var existingBoeking = command.context.Boekingen.FirstOrDefault (el => el.Id == command.id);
                            if (existingBoeking == null)
                            {
                                return Result<Boeking>.Failure ($"Boeking does not exist");
                            }
                            
                            var boeking = new Boeking { Id = command.id };
                            command.context.Attach (boeking);
                            command.context.Boekingen.Remove (boeking);
                            await transaction.CommitAsync ();

                            return Result<Boeking>.Success(boeking);
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
                    return Result<Boeking>.Failure ($"Error deleting booking: {ex.Message}");
                }
            }
        }
    }
}
