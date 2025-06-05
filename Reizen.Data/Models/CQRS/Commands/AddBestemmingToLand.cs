using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddBestemmingToLand
    {
        public record AddBestemmingToLandCommand (BestemmingDAL bestemming, LandDAL land, ReizenContext context) : ICommand<Result<BestemmingDAL>>;

        public class AddBestemmingToLandCommandHandler : ICommandHandler<AddBestemmingToLandCommand, Result<BestemmingDAL>>
        {
            public async Task<Result<BestemmingDAL>> Handle (AddBestemmingToLandCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var land = await command.context.Landen.FindAsync (command.land.Id);
                            
                            if (land == null)
                            {
                                return Result<BestemmingDAL>.Failure ($"Country does not exist"); 
                            }
                            if (land?.Bestemmingen.Where (el => el.Plaats == command.bestemming.Plaats).Count () > 0)
                            {
                                return Result<BestemmingDAL>.Failure ($"Destination already exists for this land");
                            }

                            land.Bestemmingen.Add(command.bestemming);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<BestemmingDAL>.Success(command.bestemming);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<BestemmingDAL>.Failure ($"Error adding destination to Country: {ex.Message}");
                }
            }
        }
    }
}
