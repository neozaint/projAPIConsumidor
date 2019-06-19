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
                    r = string.Empty;
                    HttpClient _apiClient = new HttpClient();
                    HttpResponseMessage _response = new HttpResponseMessage();

                    _response = await _apiClient.GetAsync("http://172.30.64.4:8080/api/Estres/saludar", cancellation);

                    if (!_response.IsSuccessStatusCode)
                    {
                        r = await _response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    r= ex.Message;
                }

            }
        }

    }
}
