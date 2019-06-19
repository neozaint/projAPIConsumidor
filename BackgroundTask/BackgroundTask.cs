using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace apiConsumidor.BackgroundTask
{
    public class BackgroundTaskConsumidor : IBackgroundTask
    {
        public CancellationTokenSource CancellationToken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public async Task<string> SaludarEscalador(CancellationToken cancellation)
        {
            string r = string.Empty;
            HttpClient _apiClient = new HttpClient();
            HttpResponseMessage _response = new HttpResponseMessage();

            _response = await _apiClient.GetAsync("http://restapiescala-escaladoros.apps.us-west-1.online-starter.openshift.com/api/Estres/saludar", cancellation);

            if (!_response.IsSuccessStatusCode)
            {
                r = await _response.Content.ReadAsStringAsync();
            }

            return r;
        }

        public Task StartAsync(IConfiguration configuration)
        {
            Task.Factory.StartNew(() => ConsumirCiclo());
            return Task.CompletedTask;
        }

        private async Task ConsumirCiclo()
        {
            string r = string.Empty;
            CancellationToken cancellation;
            while (true)
            {
                try
                {
                    r=await SaludarEscalador(cancellation);  
                }
                catch (Exception ex)
                {
                    r= ex.Message;
                }

            }
        }

    }
}
