using System.Threading.Tasks;
using MassTransit;
using Reizen.Data.Contracts;
using ReizenApi.Contracts;
using Reizen.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Reizen.Data.Consumers
{

    public class CQRSMessageConsumer(ReizenContext _context, ILogger<CQRSMessage> _logger) :
        IConsumer<CQRSMessage>
    {
        public async Task Consume(ConsumeContext<CQRSMessage> context)
        {
            ICQRSCommandContract command = context.Message as ICQRSCommandContract;
            _logger.LogInformation ($"{nameof(CQRSMessage)} : {command.Id}");
            //if(!(_context))
        }
    }
}