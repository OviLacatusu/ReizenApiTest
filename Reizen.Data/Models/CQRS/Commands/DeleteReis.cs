using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteReis
    {
        public record DeleteReisCommand(int id, ReizenContext context):ICommand<Result<ReisDAL>>;

        public class DeleteReisCommandHandler : ICommandHandler<DeleteReisCommand, Result<ReisDAL>>
        {
            public async Task<Result<ReisDAL>> Handle (DeleteReisCommand command)
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
                                return Result<ReisDAL>.Failure ($"Reis with ID not found");
                            }
                            if (existingReis.Boekingen.Where (b => b.Reis.Vertrek > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            {
                                return Result<ReisDAL>.Failure ($"Cannot delete trip with active bookings");
                            }
                            ReisDAL? reis = new ReisDAL { Id = command.id };

                            command.context.Attach (reis);
                            command.context.Reizen.Remove (reis);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<ReisDAL>.Success (reis);
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
                    return Result<ReisDAL>.Failure ($"Error deleting trip: {ex.Message}");
                }
            }
        }
    }
}
