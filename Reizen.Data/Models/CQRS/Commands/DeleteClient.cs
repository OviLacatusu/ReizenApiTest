using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteClient
    {
        public record DeleteClientCommand (int klantId, ReizenContext context) : ICommand<Result<ClientDAL>>;

        public class DeleteClientCommandHandler : ICommandHandler<DeleteClientCommand, Result<ClientDAL>>
        {
            public async Task<Result<ClientDAL>> Handle (DeleteClientCommand command)
            {
                try
                {
                    if (command.klantId < 0)
                    {
                        return Result<ClientDAL>.Failure ("Invalid id");
                    }
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var existingClient = await command.context.Clients.FindAsync (command.klantId);
                            if (existingClient == null)
                            {
                                return Result<ClientDAL>.Failure ($"Customer with ID not found");
                            }
                            if (existingClient.Bookings.Where(b => b.Trip.DateOfDeparture > DateOnly.FromDateTime(DateTime.Today)).Any())
                            {
                                return Result<ClientDAL>.Failure ($"Cannot delete customer with active bookings");
                            }
                            
                            ClientDAL klant = new ClientDAL { Id = command.klantId };

                            command.context.Attach (klant);
                            command.context.Clients.Remove (klant);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<ClientDAL>.Success(klant);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Result<ClientDAL>.Failure($"Error deleting client: {ex.Message}");
                }
}
        }     
    }
}
