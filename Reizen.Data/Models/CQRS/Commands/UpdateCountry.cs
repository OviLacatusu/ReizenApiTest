
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
    public sealed class UpdateCountry
    {
        public record UpdateCountryCommand (int id, CountryDAL countryData, ReizenContext context ) : ICommand<Result<CountryDAL>>;

        public class UpdateClassCommandHandler : ICommandHandler<UpdateCountryCommand, Result<CountryDAL>>
        {
            public async Task<Result<CountryDAL>> Handle (UpdateCountryCommand command)
            {
                try
                {
                    if (command.id < 0)
                    {
                        return Result<CountryDAL>.Failure ("Invalid id");
                    }
                    if (command.countryData is null)
                    {
                        return Result<CountryDAL>.Failure ("Iinvalid country data");
                    }
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var Country = await command.context.Countries.FindAsync (command.id);
                            if (Country == null)
                            {
                                return Result<CountryDAL>.Failure ($"Cannot find Country with ID");
                            }
                            Country.Continent = command.countryData.Continent;
                            Country.Name = command.countryData.Name;
                            Country.Destinations = Country.Destinations;

                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<CountryDAL>.Success (Country);
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
                    return Result<CountryDAL>.Failure ($"Error while updating country");
                }
                
                  
            }
        }
    }
}
