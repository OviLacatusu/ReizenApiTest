using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteKlant
    {
        public record DeleteKlantCommand (int klantId, ReizenContext context) : ICommand<Result<KlantDAL>>;

        public class DeleteKlantCommandHandler : ICommandHandler<DeleteKlantCommand, Result<KlantDAL>>
        {
            public async Task<Result<KlantDAL>> Handle (DeleteKlantCommand command)
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
                                return Result<KlantDAL>.Failure ($"Customer with ID not found");
                            }
                            if (existingKlant.Boekingen.Where(b => b.Reis.Vertrek > DateOnly.FromDateTime(DateTime.Today)).Any())
                            {
                                return Result<KlantDAL>.Failure ($"Cannot delete customer with active bookings");
                            }
                            
                            KlantDAL klant = new KlantDAL { Id = command.klantId };

                            command.context.Attach (klant);
                            command.context.Klanten.Remove (klant);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<KlantDAL>.Success(klant);
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
                    return Result<KlantDAL>.Failure($"Error deleting client: {ex.Message}");
                }
}
        }     
    }
}
