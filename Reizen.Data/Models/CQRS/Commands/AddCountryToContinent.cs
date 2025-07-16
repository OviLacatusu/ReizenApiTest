using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddCountryToContinent
    {
        public record AddCountryToContinentCommand(CountryDAL Country, ContinentDAL deel, ReizenContext context): ICommand<Result<CountryDAL>>;

        public class AddCountryToContinentCommandHandler : ICommandHandler<AddCountryToContinentCommand, Result<CountryDAL>>
        {
            public async Task<Result<CountryDAL>> Handle (AddCountryToContinentCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {

                            var deelw = await command.context.Continents.FindAsync (command.deel.Id);

                            if (deelw == null)
                            {
                                return Result<CountryDAL>.Failure ($"Continent with ID not found");
                            }

                            if (deelw?.Countries.Where (l => command.Country.Name == l.Name).Count () > 0)
                            {
                                return Result<CountryDAL>.Failure ($"Country already exists on this continent");
                            }

                            deelw.Countries.Add (command.Country);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<CountryDAL>.Success(command.Country);
                        }
                        catch
                        {
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<CountryDAL>.Failure ($"Error adding Country to continent: {ex.Message}");
                }
            }
        }
    }
}
