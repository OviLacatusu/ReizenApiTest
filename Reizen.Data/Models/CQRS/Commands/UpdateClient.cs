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
        public record UpdateClientCommand (ClientDAL klantData, int klantId) : ICommand<Result<ClientDAL>>;

        public class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, Result<ClientDAL>>
        {
            private ReizenContext _context;

            public UpdateClientCommandHandler(ReizenContext context)
            {
                _context = context;
            }
        public async Task<Result<ClientDAL>> Handle (UpdateClientCommand command)
            {
                try
                {
                //    if (command.klantData is null)
                //    {
                //        return Result<ClientDAL>.Failure ("Invalid client data");
                //    }
                //    if (command.klantId < 0)
                //    {
                //        return Result<ClientDAL>.Failure ("Invalid id");
                //    }
                    using (var transaction = await _context.Database.BeginTransactionAsync ())
                    {
                        try
                        {

                            var klant = await _context.Clients.FindAsync (command.klantId);
                            if (klant == null)
                                return Result<ClientDAL>.Failure ($"Cannot find customer with ID");

                            klant.FirstName = command.klantData.FirstName;
                            klant.FamilyName = command.klantData.FamilyName;
                            klant.Address = command.klantData.Address;

                            await _context.SaveChangesAsync ();
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
