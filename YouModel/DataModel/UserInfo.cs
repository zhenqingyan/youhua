using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YouModel.DataModel
{
    [Table("userinfo")]
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Modifier  { get; set; }
        public int IsDel { get; set; }
        public string LoginToken { get; set; }
        public int Role { get; set; }
    }
}
