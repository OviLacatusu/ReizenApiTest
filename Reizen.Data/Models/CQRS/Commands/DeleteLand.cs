using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteLand
    {
        public record DeleteLandCommand (int id, ReizenContext context) : ICommand<Result<Land>>;

        public class DeleteLandCommandHandler (int id) : ICommandHandler<DeleteLandCommand, Result<Land>>
        {
            public async Task<Result<Land>> Handle (DeleteLandCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingLand = await command.context.Klanten.FindAsync (command.id);
                            if (existingLand == null)
                            {
                                return Result<Land>.Failure ($"Country with ID not found");
                            }
                            if (existingLand.Boekingen.Where (b => b.Reis.Vertrek > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            {
                                return Result<Land>.Failure ($"Cannot delete country with active bookings");
                            }
                            Land? land = new Land { Id = command.id };
                            command.context.Attach (land);
                            command.context.Landen.Remove (land);

                            await transaction.CommitAsync ();
                            return Result<Land>.Success(land);
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
                    return Result<Land>.Failure ($"Error deleting land: {ex.Message}");
                }
}
        }
    }
}
