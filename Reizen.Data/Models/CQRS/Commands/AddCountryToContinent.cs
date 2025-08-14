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
        public record AddCountryToContinentCommand(CountryDAL country, ContinentDAL deel): ICommand<Result<CountryDAL>>;

        public class AddCountryToContinentCommandHandler : ICommandHandler<AddCountryToContinentCommand, Result<CountryDAL>>
        {
            private ReizenContext _context;

            public AddCountryToContinentCommandHandler (ReizenContext context)
            {
                _context = context;
            }
            public async Task<Result<CountryDAL>> Handle (AddCountryToContinentCommand command)
            {
                try
                {
                    //if (command.country is null)
                    //    return Result<CountryDAL>.Failure ("Invalid country data");
                    using var transaction = await _context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var deelw = await _context.Continents.FindAsync (command.deel.Id);

                            if (deelw == null)
                            {
                                return Result<CountryDAL>.Failure ($"Continent with ID not found");
                            }

                            if (deelw?.Countries.Where (l => command.country.Name == l.Name).Count () > 0)
                            {
                                return Result<CountryDAL>.Failure ($"Country already exists on this continent");
                            }

                            deelw.Countries.Add (command.country);
                            await _context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<CountryDAL>.Success(command.country);
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
