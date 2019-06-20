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
            try
            {
                HttpClient _apiClient = new HttpClient();
                HttpResponseMessage _response = new HttpResponseMessage();

                string ruta = "http://172.30.120.60:8080/api/Estres/saludar";
                
                //string ruta = "http://restapiescala-escaladoros.apps.us-west-1.online-starter.openshift.com/api/Estres/saludar";
                _response = await _apiClient.GetAsync(ruta, cancellation);

                if (_response.IsSuccessStatusCode)
                {
                    r = await _response.Content.ReadAsStringAsync();
                }
                else
                {
                    r = "Error comunicandose con la ruta " + ruta;
                }
            }
            catch (Exception ex)
            {
                r= ex.Message;
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
