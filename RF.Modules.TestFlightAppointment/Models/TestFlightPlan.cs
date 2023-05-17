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
using System.Web.Caching;
using System.Web.UI.WebControls;

namespace RF.Modules.TestFlightAppointment.Models
{
    [TableName("TestFlightPlans")]
    [PrimaryKey(nameof(FlightPlanID), AutoIncrement = true)]
    [Cacheable("TestFlightPlan", CacheItemPriority.Default, 20)]
    [Scope("ModuleId")]
    public class TestFlightPlan
    {
        public int FlightPlanID { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        [IgnoreColumn]
        public int TotalDuration => Duration + 1;

    }
}