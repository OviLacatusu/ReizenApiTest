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
        public record AddClientCommand(ClientDAL klant) : ICommand<Result<ClientDAL>>;

        public class AddClientCommandHandler : ICommandHandler<AddClientCommand, Result<ClientDAL>>
        {
            private ReizenContext _context;

            public AddClientCommandHandler (ReizenContext context)
            {
                _context = context;
            }
            public async Task<Result<ClientDAL>> Handle (AddClientCommand command)
            { 
                try
                {
                    //if (command.klant is null)
                    //    return Result<ClientDAL>.Failure ("Invalid client data");

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var result = await _context.Clients.AddAsync (command.klant);
                            await _context.SaveChangesAsync ();
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
