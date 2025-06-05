using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class UpdateKlant
    {
        public record UpdateKlantCommand (Klant klantData, int klantId, ReizenContext context) : ICommand<Result<Klant>>;

        public class UpdateKlantCommandHandler : ICommandHandler<UpdateKlantCommand, Result<Klant>>
        {
            public async Task<Result<Klant>> Handle (UpdateKlantCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {

                            var klant = await command.context.Klanten.FindAsync (command.klantId);
                            if (klant == null)
                                return Result<Klant>.Failure ($"Cannot find customer with ID");

                            klant.Voornaam = command.klantData.Voornaam;
                            klant.Familienaam = command.klantData.Familienaam;
                            klant.Adres = command.klantData.Adres;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<Klant>.Success(klant);
                        }
                        catch {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<Klant>.Failure ($"Error while updating customer");
                }
            }
        }
        
    }
}
