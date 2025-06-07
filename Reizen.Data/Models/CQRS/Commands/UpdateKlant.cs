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
    public sealed class UpdateKlant
    {
        public record UpdateKlantCommand (KlantDAL klantData, int klantId, ReizenContext context) : ICommand<Result<KlantDAL>>;

        public class UpdateKlantCommandHandler : ICommandHandler<UpdateKlantCommand, Result<KlantDAL>>
        {
            public async Task<Result<KlantDAL>> Handle (UpdateKlantCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {

                            var klant = await command.context.Klanten.FindAsync (command.klantId);
                            if (klant == null)
                                return Result<KlantDAL>.Failure ($"Cannot find customer with ID");

                            klant.Voornaam = command.klantData.Voornaam;
                            klant.Familienaam = command.klantData.Familienaam;
                            klant.Adres = command.klantData.Adres;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<KlantDAL>.Success(klant);
                        }
                        catch {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<KlantDAL>.Failure ($"Error while updating customer");
                }
            }
        }
        
    }
}
