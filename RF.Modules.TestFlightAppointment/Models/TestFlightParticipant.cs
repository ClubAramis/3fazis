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
    [TableName("TestFlightParticipants")]
    [PrimaryKey(nameof(ParticipantID), AutoIncrement = true)]
    [Scope("ModuleId")]
    public class TestFlightParticipant
    {
        public int ParticipantID { get; set; }

        public int BookingID { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public string PassengerName { get; set; }

        public string Role { get; set; }

        public string PilotLicense { get; set; }
    }
}