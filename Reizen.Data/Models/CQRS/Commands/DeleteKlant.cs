using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteKlant
    {
        public record DeleteKlantCommand (int klantId, ReizenContext context) : ICommand<Result<Klant>>;

        public class DeleteKlantCommandHandler : ICommandHandler<DeleteKlantCommand, Result<Klant>>
        {
            public async Task<Result<Klant>> Handle (DeleteKlantCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var existingKlant = await command.context.Klanten.FindAsync (command.klantId);
                            if (existingKlant == null)
                            {
                                return Result<Klant>.Failure ($"Customer with ID not found");
                            }
                            if (existingKlant.Boekingen.Where(b => b.Reis.Vertrek > DateOnly.FromDateTime(DateTime.Today)).Any())
                            {
                                return Result<Klant>.Failure ($"Cannot delete customer with active bookings");
                            }
                            
                            Klant klant = new Klant { Id = command.klantId };
                            command.context.Attach (klant);
                            command.context.Klanten.Remove (klant);

                            await transaction.CommitAsync ();

                            return Result<Klant>.Success(klant);
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
                    return Result<Klant>.Failure($"Error deleting client: {ex.Message}");
                }
}
        }     
    }
}
