using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouDal;
using YouModel.ViewModel;

namespace YouBll
{
    public class UserInfoBll
    {
        private UserInfoDal _userInfoDal;
        private MenuInfoDal _menuInfoDal;
        private RoleMenuRelationDal _roleMenuRelationDal;
        public UserInfoBll() {
            _userInfoDal = new UserInfoDal();
            _menuInfoDal = new MenuInfoDal();
            _roleMenuRelationDal = new RoleMenuRelationDal();
        }
        public UserInfoVm GetUserInfo(string accessToken)
        {
            var user = _userInfoDal.GetData("where LoginToken=@LoginToken and IsDel=0 limit 1", new
            {
                LoginToken = accessToken
            }).First();
            return new UserInfoVm()
            {
                Role = user.Role,
                UserName = user.UserName
            };
        }
        public List<MenuInfoVm> GetSimpleMenuInfo(int role)
        {
            var roleRelations = _roleMenuRelationDal.GetData("where Role=@Role and IsDel=0 limit 20", new { Role = role });
            var menuInfos = _menuInfoDal.GetData("where RowGuid in @RowGuids and IsDel=0 limit 50", new
            {
                RowGuids = roleRelations.Select(o => o.MenuGuid)
            });
            var result = new List<MenuInfoVm>();
            foreach (var item in menuInfos)
            {
                result.Add(new MenuInfoVm()
                {
                    MenuName = item.MenuName,
                    MenuUrl = item.MenuUrl,
                    SubMenuName =item.SubMenuName,
                    SubMenuUrl=item.SubMenuUrl
                });
            }
            return result;
        }
        public List<MenuInfoVm> GetMenuInfo(int role)
        {
           var roleRelations= _roleMenuRelationDal.GetData("where Role=@Role and IsDel=0 limit 20", new { Role = role });
            var menuInfos = _menuInfoDal.GetData("where RowGuid in @RowGuids and IsDel=0 limit 50", new
            {
                RowGuids=roleRelations.Select(o=>o.MenuGuid)
            });
            var menus = menuInfos.Where(o=>string.IsNullOrWhiteSpace(o.SubMenuName));
            var result = new List<MenuInfoVm>();
            foreach (var item in menus)
            {
                result.Add(new MenuInfoVm()
                {
                    MenuName=item.MenuName,
                    MenuUrl=item.MenuUrl,
                    SubInfo= menuInfos.Where(o=>o.MenuName==item.MenuName).Select(o=>new MenuInfoVm
                    {
                        SubMenuName=o.SubMenuName,
                        SubMenuUrl=o.SubMenuUrl

                    }).ToList()
                });
            }
            return result;
        }
    }
}
