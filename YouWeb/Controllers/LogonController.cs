using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;
using YouModel.ViewModel;

namespace YouWeb.Controllers
{
    public class LogonController : Controller
    {
        private LogonBll _logonBll;
        public LogonController()
        {
            _logonBll = new LogonBll();
        }
        // GET: Logon
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Logon(LogonVm logon)
        {
            var token = string.Empty;
            var result = _logonBll.checkLogon(logon,out token);
            if (result)
            {
                Request.Cookies.Clear();
                Response.Cookies.Clear();
                var cookie = new HttpCookie("accessToken");
                cookie.Value = token;
                cookie.Expires = DateTime.Now.AddHours(12);
                Response.SetCookie(cookie);
                Session["accessToken"] = token;
            }
            return Json(result);
        }

        public ActionResult ValidError()
        {
            return View();
        }
    }
}