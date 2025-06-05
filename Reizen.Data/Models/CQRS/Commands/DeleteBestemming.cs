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
        public record DeleteBestemmingCommand(string code, ReizenContext context) : ICommand<Result<Bestemming>>;

        public class DeleteBestemmingCommandHandler : ICommandHandler<DeleteBestemmingCommand, Result<Bestemming>>
        {
            public async Task<Result<Bestemming>> Handle (DeleteBestemmingCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    try
                    {
                        var bestemming = await command.context.Bestemmingen.FindAsync (command.code);
                        if (bestemming == null)
                            return Result<Bestemming>.Failure ($"Destination with code not found");
                        if (bestemming.Reizen.Where (r => r.Vertrek > DateOnly.FromDateTime (DateTime.Today)).Any ())
                            return Result<Bestemming>.Failure ($"Cannot delete destination with active trips");

                        var toDelete = new Bestemming { Code = command.code };

                        command.context.Attach (toDelete);
                        command.context.Bestemmingen.Remove (toDelete);
                        await command.context.SaveChangesAsync ();
                        await transaction.CommitAsync ();

                        return Result<Bestemming>.Success (toDelete);
                    }
                    catch
                    {
                        await transaction.RollbackAsync ();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return Result<Bestemming>.Failure ($"Error deleting customer: {ex.Message}");
                }
            }
            }
        }
    }

