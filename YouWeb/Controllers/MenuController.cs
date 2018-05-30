using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YouWeb.Controllers
{
   
    public class MenuController : BaseController
    {
        [ChildActionOnly]
        public ActionResult MenuInfo()
        {
            return PartialView(base.MenuInfos);
        }
    }
}