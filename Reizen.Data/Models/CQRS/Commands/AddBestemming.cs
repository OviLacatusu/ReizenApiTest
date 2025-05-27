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
        public record AddBestemmingCommand(ReizenContext context, Bestemming bestemming): ICommand<Result<Bestemming>>;

        public class AddBestemmingCommandHandler () : ICommandHandler<AddBestemmingCommand, Result<Bestemming>>
        {
            public async Task<Result<Bestemming>> Handle (AddBestemmingCommand command)
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
                                return Result<Bestemming>.Failure ($"Destination or the country of this destination cannot be null");
                            if (command.context.Bestemmingen.Any (b => b.Plaats == command.bestemming.Plaats))
                                return Result<Bestemming>.Failure ($"Destination already present");

                            command.context.Bestemmingen.Add (command.bestemming);
                            await transaction.CommitAsync ();
                            return Result<Bestemming>.Success (command.bestemming);
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
                    return Result<Bestemming>.Failure ($"Error adding destination: {ex.Message}");
                }
            }
        }
    }
}
