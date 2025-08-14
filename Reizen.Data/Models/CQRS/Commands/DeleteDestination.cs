using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteDestination
    {
        public record DeleteDestinationCommand(string code) : ICommand<Result<DestinationDAL>>;

        public class DeleteDestinationCommandHandler : ICommandHandler<DeleteDestinationCommand, Result<DestinationDAL>>
        {
            private ReizenContext _context;

            public DeleteDestinationCommandHandler(ReizenContext context)
            {
                _context = context;
            }
        public async Task<Result<DestinationDAL>> Handle (DeleteDestinationCommand command)
            {
                try
                {
                    //if (String.IsNullOrEmpty (command.code))
                    //{
                    //    return Result<DestinationDAL>.Failure ("Invalid code");
                    //}
                    using var transaction = await _context.Database.BeginTransactionAsync ();
                    try
                    {
                        var destinationWithCode = await _context.Destinations.FindAsync (command.code);
                        if (destinationWithCode == null)
                            return Result<DestinationDAL>.Failure ($"Destination with code not found");
                        if (destinationWithCode.Trips.Where (r => r.DateOfDeparture > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            return Result<DestinationDAL>.Failure ($"Cannot delete destination with active trips");

                        var toDelete = new DestinationDAL { Code = command.code };

                        _context.Attach (toDelete);
                        _context.Destinations.Remove (toDelete);
                        await _context.SaveChangesAsync ();
                        await transaction.CommitAsync ();

                        return Result<DestinationDAL>.Success (toDelete);
                    }
                    catch
                    {
                        await transaction.RollbackAsync ();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return Result<DestinationDAL>.Failure ($"Error deleting customer: {ex.Message}");
                }
            }
            }
        }
    }

