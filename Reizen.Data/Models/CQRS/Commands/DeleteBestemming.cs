using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class DeleteBestemming
    {
        public record DeleteBestemmingCommand(string code, ReizenContext context) : ICommand<Result<BestemmingDAL>>;

        public class DeleteBestemmingCommandHandler : ICommandHandler<DeleteBestemmingCommand, Result<BestemmingDAL>>
        {
            public async Task<Result<BestemmingDAL>> Handle (DeleteBestemmingCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    try
                    {
                        var bestemming = await command.context.Bestemmingen.FindAsync (command.code);
                        if (bestemming == null)
                            return Result<BestemmingDAL>.Failure ($"Destination with code not found");
                        if (bestemming.Reizen.Where (r => r.Vertrek > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            return Result<BestemmingDAL>.Failure ($"Cannot delete destination with active trips");

                        var toDelete = new BestemmingDAL { Code = command.code };

                        command.context.Attach (toDelete);
                        command.context.Bestemmingen.Remove (toDelete);
                        await command.context.SaveChangesAsync ();
                        await transaction.CommitAsync ();

                        return Result<BestemmingDAL>.Success (toDelete);
                    }
                    catch
                    {
                        await transaction.RollbackAsync ();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return Result<BestemmingDAL>.Failure ($"Error deleting customer: {ex.Message}");
                }
            }
            }
        }
    }

