using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddKlant
    {
        public record AddKlantCommand(Klant klant, ReizenContext context) : ICommand<Result<Klant>>;

        public class AddKlantCommandHandler : ICommandHandler<AddKlantCommand, Result<Klant>>
        {
            public async Task<Result<Klant>> Handle (AddKlantCommand command)
            { 
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            if (command.klant is null)
                                return Result<Klant>.Failure ("Klant cannot be null");
                            var result = await command.context.Klanten.AddAsync (command.klant);
                            await transaction.CommitAsync ();

                            return Result<Klant>.Success(result.Entity);
                        }
                        catch { 
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<Klant>.Failure ($"Error adding klant: {ex.Message}");
                }
            }
        }
    }
}
