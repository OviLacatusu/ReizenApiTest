using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddCountry
    {
        public record AddCountryCommand (CountryDAL Country, ReizenContext context) : ICommand<Result<CountryDAL>>;

        public class AddCountryCommandHandler : ICommandHandler<AddCountryCommand, Result<CountryDAL>> 
        {
            public async Task<Result<CountryDAL>> Handle (AddCountryCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var result = command.context.Countries.Where (l => String.Equals(command.Country.Name, l.Name, StringComparison.OrdinalIgnoreCase));
                            if (result != null)
                            {
                                return Result<CountryDAL>.Failure ($"Country already exists");
                            }

                            await command.context.Countries.AddAsync (command.Country);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<CountryDAL>.Success(command.Country);
                        }
                        catch {
                            transaction.Rollback ();
                            throw;
                        }
                    } 
                }
                catch (Exception ex) {
                    return Result<CountryDAL>.Failure ($"Error adding Country: {ex.Message}");
                }
            }
        }
    }
}
