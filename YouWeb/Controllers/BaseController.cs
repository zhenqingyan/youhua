using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;
using YouModel.ViewModel;
using YouWeb.Filter;

namespace YouWeb.Controllers
{
    [Authority]
    public class BaseController : Controller
    {
        protected UserInfoBll _userInfoBll;
        public BaseController()
        {
            _userInfoBll = new UserInfoBll();
        }
        public UserInfoVm UserInfo
        {
            get
            {
                var token = Session["accessToken"].ToString();
                return _userInfoBll.GetUserInfo(token);
            }
        }
        public List<MenuInfoVm> MenuInfos
        {
            get
            {
                return _userInfoBll.GetMenuInfo(UserInfo.Role);

            }
        }
    }
}