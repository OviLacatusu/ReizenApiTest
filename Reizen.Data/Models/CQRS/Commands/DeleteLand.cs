using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
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
        public record DeleteLandCommand (int id, ReizenContext context) : ICommand<Result<LandDAL>>;

        public class DeleteLandCommandHandler (int id) : ICommandHandler<DeleteLandCommand, Result<LandDAL>>
        {
            public async Task<Result<LandDAL>> Handle (DeleteLandCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingLand = await command.context.Landen.FindAsync (command.id);
                            if (existingLand == null)
                            {
                                return Result<LandDAL>.Failure ($"Country with ID not found");
                            }
                            if (existingLand.Bestemmingen.Where(b => b.Reizen.Where (b => b.Vertrek > DateOnly.FromDateTime (DateTime.Today)).Any()).Any ())
                            {
                                return Result<LandDAL>.Failure ($"Cannot delete country with active bookings");
                            }
                            LandDAL? land = new LandDAL { Id = command.id };

                            command.context.Attach (land);
                            command.context.Landen.Remove (land);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<LandDAL>.Success(land);
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
                    return Result<LandDAL>.Failure ($"Error deleting land: {ex.Message}");
                }
}
        }
    }
}
