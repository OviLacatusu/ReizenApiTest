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
        public record AddLandToWerelddeelCommand(LandDAL land, WerelddeelDAL deel, ReizenContext context): ICommand<Result<LandDAL>>;

        public class AddLandToWerelddeelCommandHandler : ICommandHandler<AddLandToWerelddeelCommand, Result<LandDAL>>
        {
            public async Task<Result<LandDAL>> Handle (AddLandToWerelddeelCommand command)
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
                                return Result<LandDAL>.Failure ($"Werelddeel with ID not found");
                            }

                            if (deelw?.Landen.Where (l => command.land.Naam == l.Naam).Count () > 0)
                            {
                                return Result<LandDAL>.Failure ($"Country already exists on this continent");
                            }

                            deelw.Landen.Add (command.land);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<LandDAL>.Success(command.land);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<LandDAL>.Failure ($"Error adding land to continent: {ex.Message}");
                }
            }
        }
    }
}
