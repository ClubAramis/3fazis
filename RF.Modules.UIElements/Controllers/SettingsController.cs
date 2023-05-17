/*
' Copyright (c) 2023 Adam Halassy
'  All rights reserved.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
*/

using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.UIElements.Models;
using System.Diagnostics;
using System.Web.Mvc;

namespace RF.Modules.UIElements.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Hero()
        {
            var settings = HeroSettings.Fetch(ModuleContext);

            return View(settings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="supportsTokens"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Hero(HeroSettings settings)
        {
            settings?.UpdateContext(ModuleContext);

            return RedirectToDefaultRoute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Spotlight()
        {
            var settings = SpotlightSettings.Fetch(ModuleContext);

            ViewBag.VerticalValues = new SelectList(new[]
            {
                new SelectListItem() { Text = "Top", Value = "top"},
                new SelectListItem() { Text = "Bottom", Value = "bottom"},
            }, nameof(SelectListItem.Value), nameof(SelectListItem.Text));

            ViewBag.HorizontalValues = new SelectList(new[]
            {
                new SelectListItem() { Text = "Left", Value = "left"},
                new SelectListItem() { Text = "Right", Value = "right"},
            }, nameof(SelectListItem.Value), nameof(SelectListItem.Text));

            return View(settings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="supportsTokens"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Spotlight(SpotlightSettings settings)
        {
            settings?.UpdateContext(ModuleContext);

            return RedirectToDefaultRoute();
        }
    }
}