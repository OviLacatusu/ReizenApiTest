using Reizen.Data.Repositories;
using System;
using Reizen.CommonClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddClient
    {
        public record AddClientCommand(ClientDAL klant, ReizenContext context) : ICommand<Result<ClientDAL>>;

        public class AddClientCommandHandler : ICommandHandler<AddClientCommand, Result<ClientDAL>>
        {
            public async Task<Result<ClientDAL>> Handle (AddClientCommand command)
            { 
                try
                {
                    if (command.klant is null)
                        return Result<ClientDAL>.Failure ("Invalid client data");

                    using (var transaction = await command.context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var result = await command.context.Clients.AddAsync (command.klant);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<ClientDAL>.Success(result.Entity);
                        }
                        catch { 
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<ClientDAL>.Failure ($"Error adding klant: {ex.Message}");
                }
            }
        }
    }
}
