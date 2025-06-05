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
        public record AddKlantCommand(KlantDAL klant, ReizenContext context) : ICommand<Result<KlantDAL>>;

        public class AddKlantCommandHandler : ICommandHandler<AddKlantCommand, Result<KlantDAL>>
        {
            public async Task<Result<KlantDAL>> Handle (AddKlantCommand command)
            { 
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            if (command.klant is null)
                                return Result<KlantDAL>.Failure ("Klant cannot be null");

                            var result = await command.context.Klanten.AddAsync (command.klant);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<KlantDAL>.Success(result.Entity);
                        }
                        catch { 
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<KlantDAL>.Failure ($"Error adding klant: {ex.Message}");
                }
            }
        }
    }
}
