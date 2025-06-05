using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddBestemming
    {
        public record AddBestemmingCommand(ReizenContext context, BestemmingDAL bestemming): ICommand<Result<BestemmingDAL>>;

        public class AddBestemmingCommandHandler () : ICommandHandler<AddBestemmingCommand, Result<BestemmingDAL>>
        {
            public async Task<Result<BestemmingDAL>> Handle (AddBestemmingCommand command)
            {
                var result = await command.context.Bestemmingen.AddAsync (command.bestemming);
                await command.context.SaveChangesAsync();
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            if (command.bestemming is null || command.bestemming.Land is null)
                                return Result<BestemmingDAL>.Failure ($"Destination or the country of this destination cannot be null");
                            if (command.context.Bestemmingen.Any (b => b.Plaats == command.bestemming.Plaats))
                                return Result<BestemmingDAL>.Failure ($"Destination already present");

                            command.context.Bestemmingen.Add (command.bestemming);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<BestemmingDAL>.Success (command.bestemming);
                        }
                        catch 
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
                catch (Exception ex) 
                {
                    return Result<BestemmingDAL>.Failure ($"Error adding destination: {ex.Message}");
                }
            }
        }
    }
}
