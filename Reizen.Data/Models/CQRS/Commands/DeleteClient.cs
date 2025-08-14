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
        public record DeleteClientCommand (int klantId) : ICommand<Result<ClientDAL>>;

        public class DeleteClientCommandHandler : ICommandHandler<DeleteClientCommand, Result<ClientDAL>>
        {
            private ReizenContext _context;

            public DeleteClientCommandHandler(ReizenContext context)
            {
                _context = context;
            }
        public async Task<Result<ClientDAL>> Handle (DeleteClientCommand command)
            {
                try
                {
                    //if (command.klantId < 0)
                    //{
                    //    return Result<ClientDAL>.Failure ("Invalid id");
                    //}
                    using var transaction = await _context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var existingClient = await _context.Clients.FindAsync (command.klantId);
                            if (existingClient == null)
                            {
                                return Result<ClientDAL>.Failure ($"Customer with ID not found");
                            }
                            if (existingClient.Bookings.Where(b => b.Trip.DateOfDeparture > DateOnly.FromDateTime(DateTime.Today)).Any())
                            {
                                return Result<ClientDAL>.Failure ($"Cannot delete customer with active bookings");
                            }
                            
                            ClientDAL klant = new ClientDAL { Id = command.klantId };

                            _context.Attach (klant);
                            _context.Clients.Remove (klant);
                            await _context.SaveChangesAsync ();
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
