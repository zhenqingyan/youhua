using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.ViewModel
{
    public class MenuInfoVm
    {
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuUrl { get; set; }
        public List<MenuInfoVm> SubInfo { get; set; }
    }
}
