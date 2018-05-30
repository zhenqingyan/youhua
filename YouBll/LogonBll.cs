using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouModel.ViewModel;
using YouDal;

namespace YouBll
{
    public class LogonBll
    {
        public UserInfoDal _userInfoDal;
        public LogonBll()
        {
            _userInfoDal = new UserInfoDal();
        }
        public bool checkLogon(LogonVm logon,out string token)
        {
            token = string.Empty;
            var userInfo=_userInfoDal.GetData("where LoginName=@LoginName  and PassWord=@PassWord limit 10", new
            {
                LoginName = logon.Name,
                PassWord = logon.Password
            });
            var checkResult = userInfo != null && userInfo.Count() == 1;
            if (checkResult)
            {
                token = Guid.NewGuid().ToString();
                var user = userInfo.First();
                user.LoginToken = token;
                user.ModifyTime = DateTime.Now;
                _userInfoDal.Update(user);
            }
            return checkResult;
        }
    }
}
