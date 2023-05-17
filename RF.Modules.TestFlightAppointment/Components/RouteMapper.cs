using DotNetNuke.Web.Api;
using System.Diagnostics;

namespace RF.Modules.TestFlightAppointment.Components
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute(
                "TestFlightBooking",
                "default",
                "{controller}/{action}",
                new string[] { "RF.Modules.TestFlightAppointment.Controllers.Api" });
        }
    }
}