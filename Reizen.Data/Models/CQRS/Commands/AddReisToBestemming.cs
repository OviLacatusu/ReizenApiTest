using Microsoft.EntityFrameworkCore.Diagnostics;
using Reizen.Data.Repositories;
using Reizen.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddReisToBestemming
    {
        public record AddReisToBestemmingCommand(ReisDAL reis, BestemmingDAL bestemming, ReizenContext context):ICommand<Result<ReisDAL>>;

        public class AddReisToBestemmingCommandHandler : ICommandHandler<AddReisToBestemmingCommand, Result<ReisDAL>>
        {
            public async Task<Result<ReisDAL>> Handle (AddReisToBestemmingCommand command)
            {
                try
                {
                    using var transaction = await command.context.Database.BeginTransactionAsync ();
                    {
                        try
                        {
                            var existingBestemming = await command.context.Bestemmingen.FindAsync (command.bestemming.Code);

                            if (command.reis == null || command.bestemming == null)
                                return Result<ReisDAL>.Failure ($"Trip or destination cannot be null");
                            if (command.reis.Vertrek < DateOnly.FromDateTime (DateTime.Today))
                                return Result<ReisDAL>.Failure ($"Trip departure cannot be in the past");
                            if (existingBestemming is null)
                                return Result<ReisDAL>.Failure ($"Destination not found");
                            if (command.reis.Bestemmingscode != command.bestemming.Code)
                                return Result<ReisDAL>.Failure ($"Destination code does not match trip destination code");

                            existingBestemming?.Reizen.Add (command.reis);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();

                            return Result<ReisDAL>.Success(command.reis);
                        }
                        catch { 
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
                catch (Exception ex) {
                    return Result<ReisDAL>.Failure ($"Error adding Reis to destination: {ex.Message}");
                }
            }
        }
    }
}
