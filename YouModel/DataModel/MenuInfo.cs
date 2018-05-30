using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.DataModel
{
    [Table("menuinfo")]
    public class MenuInfo
    {
        [Key]
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuUrl { get; set; }
        public int IsDel { get; set; }
    }
}
