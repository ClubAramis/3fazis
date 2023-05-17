using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.TestFlightAppointment.Models;
using RF.Modules.TestFlightAppointment.Services;
using RF.Modules.TestFlightAppointment.Util;
using System;
using System.Web.Mvc;

namespace RF.Modules.TestFlightAppointment.Controllers
{
    [DnnHandleError]
    public class TestFlightGridController : DnnController
    {
        private ITestFlightBookingManager BookingManager { get; }

        public TestFlightGridController(
            ITestFlightBookingManager bookingManager
            )
        {
            BookingManager = bookingManager
                ?? throw new ArgumentNullException(nameof(bookingManager));
        }
        private static void InitPopup()
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.jQuery);
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);
            ServicesFramework.Instance.RequestAjaxScriptSupport();
        }

        [HttpGet]
        public ActionResult Index(int? year, int? week)
        {
            var weekOfYear = year.HasValue && week.HasValue
                ? new WeekOfYear(year.Value, week.Value)
                : new WeekOfYear(DateTime.UtcNow);

            var utcNow = DateTime.UtcNow;
            var bookings = BookingManager.FindBookingsByDate(weekOfYear.FirstDay, weekOfYear.LastDay, false);
            ViewBag.Bookings = bookings;
            ViewBag.WeekOfYear = weekOfYear;

            return View();
        }

        [HttpGet]
        public ActionResult Create(DateTime? departureAt)
        {
            InitPopup();

            var model = new CreateBookingParameters()
            {
                DepartureAt = departureAt ?? DateTime.UtcNow,
            };

            ViewBag.Plans = BookingManager.FindFlightPlans(
                User.IsAdmin
                );
            ViewBag.PassengerTypes = new SelectList(new[]
            {
                new SelectListItem() { Text = "-- select --", Value = null, Selected = true },
                new SelectListItem() { Text = "Passenger", Value = "passenger", },
                new SelectListItem() { Text = "Pilot", Value = "pilot" },
                new SelectListItem() { Text = "Engineer", Value = "engineer" },
            }, nameof(SelectListItem.Value), nameof(SelectListItem.Text));

            return PartialView("Create", model);
        }

        [HttpGet]
        public ActionResult Detail(int bookingID)
        {
            InitPopup();

            var booking = BookingManager.FindBookingByID(bookingID);
            return View(booking);
        }
    }
}