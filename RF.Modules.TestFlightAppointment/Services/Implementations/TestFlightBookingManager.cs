using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.Api;
using RF.Modules.TestFlightAppointment.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace RF.Modules.TestFlightAppointment.Services.Implementations
{
    internal class TestFlightBookingManager : ITestFlightBookingManager
    {
        public TestFlightBookingManager()
            : this(DotNetNuke.Entities.Users.UserController.Instance)
        { }


        public TestFlightBookingManager(
            IUserController userController
            )
        {
            UserController = userController
                ?? throw new ArgumentNullException(nameof(userController));
        }

        private IUserController UserController { get; }

        private bool HasAccess(TestFlightBooking booking)
        {
            var currentUser = UserController.GetCurrentUserInfo();
            if (currentUser is null || currentUser.UserID == Null.NullInteger)
                return false;

            return currentUser.IsAdmin
                || currentUser.UserID == booking.CreatedByUserID;
        }

        private void AssertAccess(TestFlightBooking booking)
        {
            if (!HasAccess(booking))
                throw new TestFlightException("Permission denied.");
        }

        private BookingData[] FetchBookingData(IDataContext ctx, TestFlightBooking[] bookings)
        {
            var currentUser = UserController.GetCurrentUserInfo();
            var planIDs = bookings.Select(b => b.FlightPlanID)
                .Distinct()
                .ToArray();

            var plans = planIDs.Length == 0
                ? new TestFlightPlan[0]
                : ctx.GetRepository<TestFlightPlan>()
                    .Find("WHERE FlightPlanID in (@0)", planIDs)
                    .ToArray();

            var bookingIDs = bookings.Select(b => b.BookingID)
                .Distinct()
                .ToArray();
            var participants = bookingIDs.Length == 0
                ? new TestFlightParticipant[0] 
                : ctx.GetRepository<TestFlightParticipant>()
                    .Find("WHERE BookingID in (@0)", bookingIDs)
                    .ToArray();

            return bookings
                .Select(b => new BookingData(
                    b,
                    plans.FirstOrDefault(p => p.FlightPlanID == b.FlightPlanID),
                    participants.Where(p => p.BookingID == b.BookingID).ToArray()
                    )
                {
                    User = currentUser.IsAdmin
                        ? UserController.GetUserById(currentUser.PortalID, b.CreatedByUserID)
                        : (b.CreatedByUserID == currentUser.UserID ? currentUser : null)
                })
                .ToArray();
        }

        public TestFlightParticipant AddParticipantTo(
            int bookingID,
            TestFlightParticipant participant
            )
        {
            var currentUser = UserController.GetCurrentUserInfo();
            if (currentUser.UserID == Null.NullInteger)
                throw new TestFlightException("Guests can't create booking.");
            
            var booking = FindBookingByID(bookingID);
            if (booking is null)
                throw new ApplicationException("Booking not found.");

            AssertAccess(booking.Booking);

            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightParticipant>();

                participant.BookingID = booking.Booking.BookingID;
                participant.CreatedByUserID = currentUser.UserID;
                participant.CreatedOnDate = DateTime.Now;
                r.Insert(participant);

                return participant;
            }
        }

        public void CancelBooking(int bookingID)
        {
            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightBooking>();
                var booking = r.GetById(bookingID);
                if (booking != null)
                {
                    AssertAccess(booking);
                    booking.IsCancelled = true;
                    r.Update(booking);
                }
            }
        }

        public TestFlightBooking CreateBooking(TestFlightBooking booking)
        {
            var plan = FindPlanByID(booking.FlightPlanID);

            var currentUser = UserController.GetCurrentUserInfo();
            if (currentUser.UserID == Null.NullInteger)
                throw new TestFlightException("Guests can't create booking.");

            var existingBookings = FindBookingsByDate(
                booking.DepartureAt,
                booking.DepartureAt.AddHours(plan.TotalDuration),
                false
                );
            if (existingBookings.Length != 0)
                throw new ApplicationException("There is an other booking in the selected slot.");

            booking.Duration = plan.TotalDuration;
            booking.CreatedByUserID = currentUser.UserID;
            booking.CreatedOnDate = DateTime.Now;

            using (var ctx = DataContext.Instance())
            {

                var r = ctx.GetRepository<TestFlightBooking>();
                r.Insert(booking);
            }

            return booking;
        }

        public BookingData FindBookingByID(int bookingID)
        {
            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightBooking>();
                var booking = r.GetById(bookingID);
                if (booking is null || !HasAccess(booking))
                    return null;

                var plan = ctx.GetRepository<TestFlightPlan>()
                    .GetById(booking.FlightPlanID);

                var participants = ctx.GetRepository<TestFlightParticipant>()
                    .Find("WHERE BookingID = @0", bookingID)
                    .ToArray();

                return new BookingData(booking, plan, participants);
            }
        }

        public TestFlightPlan FindPlanByID(int planID)
        {
            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightPlan>();
                return r.GetById(planID);
            }
        }

        public BookingData[] FindBookingsByDate(DateTime? from, DateTime? to, bool findAll)
        {
            using (var ctx = DataContext.Instance())
            {
                var actualFrom = from ?? DateTime.MinValue;
                var actualTo = to ?? DateTime.MaxValue;

                var result = ctx.GetRepository<TestFlightBooking>()
                    .Find(
                        "WHERE @0 < DepartureAt AND DepartureAt < @1 AND (IsCancelled = 0 OR @2 = 1)",
                        actualFrom,
                        actualTo,
                        findAll
                        )
                    .ToArray();
                return FetchBookingData(ctx, result);
            }
        }

        public BookingData[] FindBookingsByUser(int userID, DateTime? from, DateTime? to)
        {
            using (var ctx = DataContext.Instance())
            {
                var actualFrom = from ?? DateTime.MinValue;
                var actualTo = to ?? DateTime.MaxValue;

                var result = ctx.GetRepository<TestFlightBooking>()
                    .Find(
                        "WHERE @0 <= DepartureAt AND DepartureAt <= @1 AND CreatedByUserID = @2",
                        actualFrom,
                        actualTo,
                        userID
                        )
                    .ToArray();
                return FetchBookingData(ctx, result);
            }
        }

        public TestFlightPlan[] FindFlightPlans(bool findAll)
        {
            using (var ctx = DataContext.Instance())
            {
                return ctx.GetRepository<TestFlightPlan>()
                    .Find("WHERE IsPublic = 1 OR @0 = 1", findAll)
                    .ToArray();
            }
        }

        public bool IsSlotAvailable(DateTime from, int duration)
        {
            using (var ctx = DataContext.Instance())
            {
                var to = from.AddHours(duration);

                var results = ctx.GetRepository<TestFlightBooking>()
                    .Find(
                        "WHERE @0 <= DATEADD(HOUR, Duration, DepartureAt) AND DepartureAt <= @1 AND IsCancelled = 0",
                        from,
                        to
                        )
                    .ToArray();

                return results.Length == 0;
            }
        }

    }
}