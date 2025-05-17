using GoogleAccess.Domain.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ReizenWebBlazor.Client.Models;
using System.ComponentModel;

namespace ReizenWebBlazor.Client.Services
{
    public class PollingAPIService (IServiceProvider _provider, ILogger<PollingAPIService> _logger) : BackgroundService
    {

        private readonly Object _stateLock = new ();

        private int counter = 1;

        private PollingConfig _state = new();
        protected override async Task ExecuteAsync (CancellationToken stoppingToken)
        {
            //var _state = _provider.GetService<PollingConfig> ();

            _logger.LogInformation ($"Running for the {counter} time at {DateTime.UtcNow.ToShortTimeString()}");

            while (_state?.RequestUrl is null)
            {
                counter++;
                await Task.Delay (3000);
                _logger.LogInformation ($"Running for the {counter} time at {DateTime.UtcNow.ToShortTimeString ()} - delaying 3 seconds");
            }
            var RequestUrl = _state.RequestUrl;
            HttpClient client = new HttpClient { BaseAddress = new Uri(RequestUrl) };

            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay (3000,stoppingToken);

                var response = await client.GetAsync (RequestUrl, stoppingToken);
                var result = await response.Content.ReadAsStringAsync ();
                var session = JsonConvert.DeserializeObject<PickingSession> (result);

                if (session?.mediaItemsSet == true)
                {
                    break;
                }
            }
        }
        public void UpdateState (Action<PollingConfig> updateAction)
        {
            lock (_stateLock)
            {
                updateAction (_state);
            }
        }

        public PollingConfig GetConfig ()
        {
            lock (_stateLock)
            {
                return new PollingConfig
                {
                    RequestUrl = _state.RequestUrl
                };
            }
        }
    }
}
