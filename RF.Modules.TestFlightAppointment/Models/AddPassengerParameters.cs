using System.Text.RegularExpressions;

namespace RF.Modules.TestFlightAppointment.Models
{
    public class AddPassengerParameters
    {
        public int BookingID { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string License { get; set; }

        public bool Validate()
        {
            var regex = new Regex(@"([a-zA-Z\d]{3})-(\d{5})-(\d{3})-(\d{4})-[sSmMlLcC]");
            return !string.IsNullOrWhiteSpace(Role) 
                && !string.IsNullOrWhiteSpace(Name)
                && (!"pilot".Equals(Role) || regex.IsMatch(License));
        }
    }
}