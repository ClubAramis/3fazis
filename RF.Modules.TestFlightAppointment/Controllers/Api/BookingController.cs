using RF.Modules.TestFlightAppointment.Models;
using RF.Modules.TestFlightAppointment.Services;
using RF.Modules.TestFlightAppointment.Util;
using System;
using System.Net.Http;
using System.Web.Http;

namespace RF.Modules.TestFlightAppointment.Controllers.Api
{
    public class BookingController : BookingApiControllerBase
    {
        public BookingController(
            ITestFlightBookingManager bookingManager
            ) : base(bookingManager) { }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var booking = BookingManager.FindBookingByID(id);
                return Json(new { booking });
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage List(int year, int week)
        {
            try
            {
                var from = DateUtil.FirstDateOfWeekISO8601(year, week);
                var to = from.AddDays(8).AddSeconds(-1);

                var bookings = BookingManager.FindBookingsByDate(
                    from,
                    to,
                    false
                    );

                return Json(new { from, to, bookings });
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage Create([FromBody] CreateBookingParameters args)
        {
            try
            {
                var booking = new TestFlightBooking()
                {
                    DepartureAt = args.DepartureAt,
                    FlightPlanID = args.PlanID
                };

                booking = BookingManager.CreateBooking(
                    booking
                    );

                return Json(booking);
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage Cancel([FromBody] CancelBookingParameters args)
        {
            try
            {
                BookingManager.CancelBooking(args.BookingID);

                return JsonOk();
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AddPassengerParameters args)
        {
            try
            {
                if (!args.Validate())
                    return Json(401, "Invalid participant data.");

                var participant = new TestFlightParticipant()
                {
                    PassengerName = args.Name,
                    Role = args.Role,
                    PilotLicense = args.License,
                };

                participant = BookingManager.AddParticipantTo(args.BookingID, participant);

                return Json(participant);
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }
    }
}