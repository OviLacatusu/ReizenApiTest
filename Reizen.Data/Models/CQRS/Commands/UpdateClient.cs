using Microsoft.EntityFrameworkCore;
using Reizen.CommonClasses;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateClient
    {
        public record UpdateClientCommand (ClientDAL klantData, int klantId, ReizenContext context) : ICommand<Result<ClientDAL>>;

        public class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, Result<ClientDAL>>
        {
            public async Task<Result<ClientDAL>> Handle (UpdateClientCommand command)
            {
                try
                {
                    if (command.klantData is null)
                    {
                        return Result<ClientDAL>.Failure ("Invalid client data");
                    }
                    if (command.klantId < 0)
                    {
                        return Result<ClientDAL>.Failure ("Invalid id");
                    }
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {

                            var klant = await command.context.Clients.FindAsync (command.klantId);
                            if (klant == null)
                                return Result<ClientDAL>.Failure ($"Cannot find customer with ID");

                            klant.FirstName = command.klantData.FirstName;
                            klant.FamilyName = command.klantData.FamilyName;
                            klant.Address = command.klantData.Address;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<ClientDAL>.Success(klant);
                        }
                        catch {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<ClientDAL>.Failure ($"Error while updating customer");
                }
            }
        }
        
    }
}
