
using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateLand
    {
        public record UpdateLandCommand (int id, Land landData, ReizenContext context ) : ICommand<Result<Land>>;

        public class UpdateClassCommandHandler : ICommandHandler<UpdateLandCommand, Result<Land>>
        {
            public async Task<Result<Land>> Handle (UpdateLandCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var land = await command.context.Landen.FindAsync (command.id);
                            if (land == null)
                            {
                                return Result<Land>.Failure ($"Cannot find land with ID");
                            }
                            land.Werelddeel = command.landData.Werelddeel;
                            land.Naam = command.landData.Naam;
                            land.Bestemmingen = land.Bestemmingen;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<Land>.Success (land);
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
                    return Result<Land>.Failure ($"Error while updating country");
                }
                
                  
            }
        }
    }
}
