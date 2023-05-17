using RF.Modules.TestFlightAppointment.Services;
using System;

namespace RF.Modules.TestFlightAppointment.Controllers.Api
{

    public class BookingApiControllerBase : RestApiControllerBase
    {
        protected ITestFlightBookingManager BookingManager { get; }

        public BookingApiControllerBase(
            ITestFlightBookingManager bookingManager
            )
        {
            BookingManager = bookingManager
                ?? throw new ArgumentNullException(nameof(bookingManager));
        }
    }
}