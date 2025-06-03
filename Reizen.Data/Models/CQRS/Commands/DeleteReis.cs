using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteReis
    {
        public record DeleteReisCommand(int id, ReizenContext context):ICommand<Result<Reis>>;

        public class DeleteReisCommandHandler : ICommandHandler<DeleteReisCommand, Result<Reis>>
        {
            public async Task<Result<Reis>> Handle (DeleteReisCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingReis = await command.context.Reizen.FindAsync (command.id);
                            if (existingReis == null)
                            {
                                return Result<Reis>.Failure ($"Reis with ID not found");
                            }
                            if (existingReis.Boekingen.Where (b => b.Reis.Vertrek > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            {
                                return Result<Reis>.Failure ($"Cannot delete trip with active bookings");
                            }
                            Reis? reis = new Reis { Id = command.id };
                            command.context.Attach (reis);
                            command.context.Reizen.Remove (reis);

                            await transaction.CommitAsync ();
                            return Result<Reis>.Success (reis);
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
                    return Result<Reis>.Failure ($"Error deleting trip: {ex.Message}");
                }
            }
        }
    }
}
