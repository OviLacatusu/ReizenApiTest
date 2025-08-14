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
        public record AddCountryCommand (CountryDAL country) : ICommand<Result<CountryDAL>>;

        public class AddCountryCommandHandler : ICommandHandler<AddCountryCommand, Result<CountryDAL>> 
        {
            private ReizenContext _context;

            public AddCountryCommandHandler (ReizenContext context)
            {
                _context = context;
            }
            public async Task<Result<CountryDAL>> Handle (AddCountryCommand command)
            {
                try
                {
                    //if (command.country is null)
                    //{
                    //    return Result<CountryDAL>.Failure ("Invalid country data");
                    //}
                    using (var transaction = await _context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var result = _context.Countries.Where (l => String.Equals(command.country.Name, l.Name, StringComparison.OrdinalIgnoreCase));
                            if (result != null)
                            {
                                return Result<CountryDAL>.Failure ($"Country already exists");
                            }

                            await _context.Countries.AddAsync (command.country);
                            await _context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<CountryDAL>.Success(command.country);
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
