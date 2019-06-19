using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace apiConsumidor.BackgroundTask
{
    public interface IBackgroundTask
    {
        //
        // Summary:
        //     Triggered when the application host is ready to start the service.
        Task StartAsync(IConfiguration configuration);
    }
}
