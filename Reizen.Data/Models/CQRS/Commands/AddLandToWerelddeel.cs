using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddLandToWerelddeel
    {
        public record AddLandToWerelddeelCommand(Land land, Werelddeel deel, ReizenContext context): ICommand<Result<Land>>;

        public class AddLandToWerelddeelCommandHandler : ICommandHandler<AddLandToWerelddeelCommand, Result<Land>>
        {
            public async Task<Result<Land>> Handle (AddLandToWerelddeelCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {

                            var deelw = await command.context.Werelddelen.FindAsync (command.deel.Id);

                            if (deelw == null)
                            {
                                return Result<Land>.Failure ($"Werelddeel with ID not found");
                            }

                            if (deelw?.Landen.Where (l => command.land.Naam == l.Naam).Count () > 0)
                            {
                                return Result<Land>.Failure ($"Country already exists on this continent");
                            }
                            deelw.Landen.Add (command.land);
                            await transaction.CommitAsync ();
                            return Result<Land>.Success(command.land);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<Land>.Failure ($"Error adding land to continent: {ex.Message}");
                }
            }
        }
    }
}
