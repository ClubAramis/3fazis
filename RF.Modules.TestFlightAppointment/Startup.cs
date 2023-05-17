using DotNetNuke.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RF.Modules.TestFlightAppointment.Services;
using RF.Modules.TestFlightAppointment.Services.Implementations;

namespace RF.Modules.TestFlightAppointment
{
    public class Startup : IDnnStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITestFlightBookingManager, TestFlightBookingManager>();
        }
    }
}