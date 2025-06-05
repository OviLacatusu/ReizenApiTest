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
        public record AddBoekingCommand(BoekingDAL boeking, ReizenContext context) : ICommand<Result<BoekingDAL>>;

        public class AddBoekingCommandHandler : ICommandHandler<AddBoekingCommand, Result<BoekingDAL>>
        {
            public async Task<Result<BoekingDAL>> Handle (AddBoekingCommand command)
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
                                return Result<BoekingDAL>.Failure ("Boeking already exists for this client");
                            }
                            var result = await command.context.Boekingen.AddAsync (command.boeking);
                            await command.context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            
                            return Result<BoekingDAL>.Success(command.boeking);
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
                    return Result<BoekingDAL>.Failure ($"Error adding booking: {ex.Message}");
                }
                
            }
        }
    }
}
