using System;

namespace RF.Modules.TestFlightAppointment.Models
{

    public class CreateBookingParameters
    {
        public int PlanID { get; set; }

        public DateTime DepartureAt { get; set; }
    }
}