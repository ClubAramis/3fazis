using DotNetNuke.Entities.Users;
using DotNetNuke.UI.UserControls;
using RF.Modules.TestFlightAppointment.Models;
using System;

namespace RF.Modules.TestFlightAppointment.Services
{
    public class BookingData
    {
        public int BookingID => Booking.BookingID;

        public int Duration => Booking.Duration;

        public TestFlightBooking Booking { get; }

        public TestFlightParticipant[] Participants { get; }

        public TestFlightPlan Plan { get; }

        public UserInfo User { get; internal set; }

        internal BookingData(
            TestFlightBooking booking,
            TestFlightPlan plan,
            TestFlightParticipant[] participants
            )
        {
            Booking = booking
                ?? throw new ArgumentNullException(nameof(booking));
            Plan = plan
                ?? throw new ArgumentNullException(nameof(plan));
            Participants = participants
                ?? throw new ArgumentNullException(nameof(participants));
        }

        public bool IsBookedAt(DateTime dateTime, int duration)
            => Booking.IsBookedAt(dateTime, duration);
    }
}