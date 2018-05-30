using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouDal;
using YouModel.ViewModel;

namespace YouBll
{
    public class AuthorityBll
    {
        private UserInfoDal _userInfoDal;
        private MenuInfoDal _menuInfoDal;
        private RoleMenuRelationDal _roleMenuRelationDal;
        public AuthorityBll()
        {
            _userInfoDal = new UserInfoDal();
            _menuInfoDal = new MenuInfoDal();
            _roleMenuRelationDal = new RoleMenuRelationDal();
        }
        public bool IsValidUser(string token)
        {
            var users=_userInfoDal.GetData("where LoginToken=@LoginToken and IsDel=0 limit 1",new {
                LoginToken= token
            });

            return users != null & users.Count() == 1;
        }

       
    }
}
