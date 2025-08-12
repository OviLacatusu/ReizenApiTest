using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddDestination
    {
        public record AddDestinationCommand(ReizenContext context, DestinationDAL Destination): ICommand<Result<DestinationDAL>>;

        public class AddDestinationCommandHandler () : ICommandHandler<AddDestinationCommand, Result<DestinationDAL>>
        {
            public async Task<Result<DestinationDAL>> Handle (AddDestinationCommand command)
            {
                if (command.Destination is null || command.Destination.Country is null)
                                return Result<DestinationDAL>.Failure ($"Destination or the country of this destination cannot be null");
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync())
                    {
                        try
                        { 
                            if (command.context.Destinations.Any (b => b.PlaceName == command.Destination.PlaceName))
                                return Result<DestinationDAL>.Failure ($"Destination already present");

                            command.context.Destinations.Add (command.Destination);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<DestinationDAL>.Success (command.Destination);
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
                    return Result<DestinationDAL>.Failure ($"Error adding destination: {ex.Message}");
                }
            }
        }
    }
}
