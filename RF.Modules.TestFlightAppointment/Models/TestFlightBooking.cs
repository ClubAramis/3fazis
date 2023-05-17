/*
' Copyright (c) 2023 BCE
'  All rights reserved.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
*/

using DotNetNuke.ComponentModel.DataAnnotations;
using System;

namespace RF.Modules.TestFlightAppointment.Models
{
    [TableName("TestFlightBookings")]
    [PrimaryKey(nameof(BookingID), AutoIncrement = true)]
    public class TestFlightBooking
    {
        public int BookingID { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime DepartureAt { get; set; }

        public int Duration { get; set; }

        public int FlightPlanID { get; set; }

        public bool IsBookedAt(DateTime dateTime, int duration)
        {
            var periodEnd = dateTime.AddHours(duration);
            var flightEnd = DepartureAt.AddHours(Duration);
            return DepartureAt < periodEnd && dateTime < flightEnd;

        }
    }
}