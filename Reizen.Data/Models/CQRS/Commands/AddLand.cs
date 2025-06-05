using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddLand
    {
        public record AddLandCommand (Land land, ReizenContext context) : ICommand<Result<Land>>;

        public class AddLandCommandHandler : ICommandHandler<AddLandCommand, Result<Land>> 
        {
            public async Task<Result<Land>> Handle (AddLandCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var result = command.context.Landen.Where (l => String.Equals(command.land.Naam, l.Naam, StringComparison.OrdinalIgnoreCase));
                            if (result != null)
                            {
                                return Result<Land>.Failure ($"Land already exists");
                            }

                            await command.context.Landen.AddAsync (command.land);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<Land>.Success(command.land);
                        }
                        catch {
                            transaction.Rollback ();
                            throw;
                        }
                    } 
                }
                catch (Exception ex) {
                    return Result<Land>.Failure ($"Error adding Land: {ex.Message}");
                }
            }
        }
    }
}
