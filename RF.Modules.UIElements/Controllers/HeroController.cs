using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.UIElements.Models;
using System.Web.Mvc;

namespace RF.Modules.UIElements.Controllers
{
    [DnnHandleError]
    public class HeroController : DnnController
    {
        public ActionResult Index()
            => View(HeroSettings.Fetch(ModuleContext));
    }
}