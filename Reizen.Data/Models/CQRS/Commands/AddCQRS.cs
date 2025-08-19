using MassTransit.DependencyInjection;
using Reizen.CommonClasses;
using Reizen.Data.Contracts;
using Reizen.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models.CQRS.Commands
{
    public sealed class AddCQRS
    {
        public record AddCQRSCommand (CQRSMessage message) : ICommand<Result<CQRSMessage>>;

        public class AddCQRSCommandHandler : ICommandHandler<AddCQRSCommand, Result<CQRSMessage>>
        {
            private ReizenContext _context;

            public AddCQRSCommandHandler (ReizenContext context)
            {
                _context = context;
            } 
            public async Task<Result<CQRSMessage>> Handle (AddCQRSCommand command)
            {
                try
                {
                    using (var transaction = await _context.Database.BeginTransactionAsync ())
                    {
                        try
                        {
                            var result = await _context.OutboxCQRSMessages.AddAsync (command.message);
                            await _context.SaveChangesAsync ();
                            await transaction.CommitAsync ();
                            return Result<CQRSMessage>.Success (command.message);
                        }
                        catch (Exception) 
                        {
                            transaction.Rollback ();
                            throw;
                        }
                        
                    }
                }
                catch (Exception ex) {
                    return Result<CQRSMessage>.Failure ($"Error adding CQRS command: {ex.Message}");
                } 
            }
        }
    }
}
