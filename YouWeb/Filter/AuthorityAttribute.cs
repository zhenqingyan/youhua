using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;

namespace YouWeb.Filter
{
    public class AuthorityAttribute : FilterAttribute, IActionFilter
    {
        private AuthorityBll _authorityBll;
        private UserInfoBll _userInfoBll;

        public AuthorityAttribute()
        {
            _authorityBll = new AuthorityBll();
            _userInfoBll = new UserInfoBll();
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["accessToken"] == null)
            {
                var cookies = filterContext.HttpContext.Request.Cookies;
                if (cookies.AllKeys.Any(o => o == "accessToken"))
                {
                    var token = cookies["accessToken"].Value;
                    if (!_authorityBll.IsValidUser(token))
                    {
                        filterContext.HttpContext.Response.Redirect("/Logon/Index");
                    }
                    else
                    {
                        filterContext.HttpContext.Session["accessToken"] = token;
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/Logon/Index");
                }
            }
        }
    }
}