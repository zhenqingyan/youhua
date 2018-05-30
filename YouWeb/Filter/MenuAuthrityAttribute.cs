using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;

namespace YouWeb.Filter
{
    public class MenuAuthrityAttribute : FilterAttribute, IActionFilter
    {
        private UserInfoBll _userInfoBll;
        public MenuAuthrityAttribute()
        {
            _userInfoBll = new UserInfoBll();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userInfo = _userInfoBll.GetUserInfo(filterContext.HttpContext.Session["accessToken"].ToString());
            if (!_userInfoBll.GetSimpleMenuInfo(userInfo.Role).Any(o => filterContext.HttpContext.Request.Url.AbsolutePath.Equals(o.SubMenuUrl))
                && !filterContext.HttpContext.Request.Url.AbsolutePath.Equals("/Logon/ValidError"))
            {
                filterContext.HttpContext.Response.Redirect("/Logon/ValidError");
            }
        }
    }
}