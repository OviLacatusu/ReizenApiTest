using Microsoft.EntityFrameworkCore;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddBoeking
    {
        public record AddBoekingCommand(Boeking boeking, ReizenContext context) : ICommand<Result<Boeking>>;

        public class AddBoekingCommandHandler : ICommandHandler<AddBoekingCommand, Result<Boeking>>
        {
            public async Task<Result<Boeking>> Handle (AddBoekingCommand command)
            {
                try
                {
                    using (var transaction = await command.context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var existingBoeking = await command.context.Boekingen.Where (b => b.Klantid == command.boeking.Klantid && b.Reisid == command.boeking.Reisid).ToListAsync ();

                            if (existingBoeking.Any ())
                            {
                                await transaction.RollbackAsync ();
                                return Result<Boeking>.Failure ("Boeking already exists for this client");
                            }
                            var result = await command.context.Boekingen.AddAsync (command.boeking);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            
                            return Result<Boeking>.Success(command.boeking);
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
                    return Result<Boeking>.Failure ($"Error adding booking: {ex.Message}");
                }
                
            }
        }
    }
}
