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
        public record AddBestemmingToLandCommand (Bestemming bestemming, Land land, ReizenContext context) : ICommand<Result<Bestemming>>;

        public class AddBestemmingToLandCommandHandler : ICommandHandler<AddBestemmingToLandCommand, Result<Bestemming>>
        {
            public async Task<Result<Bestemming>> Handle (AddBestemmingToLandCommand command)
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
                                return Result<Bestemming>.Failure ($"Country does not exist"); 
                            }
                            if (land?.Bestemmingen.Where (el => el.Plaats == command.bestemming.Plaats).Count () > 0)
                            {
                                return Result<Bestemming>.Failure ($"Destination already exists for this land");
                            }

                            land.Bestemmingen.Add(command.bestemming);
                            await transaction.CommitAsync ();

                            return Result<Bestemming>.Success(command.bestemming);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<Bestemming>.Failure ($"Error adding destination to Country: {ex.Message}");
                }
            }
        }
    }
}
