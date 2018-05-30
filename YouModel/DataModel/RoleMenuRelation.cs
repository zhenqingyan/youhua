using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.DataModel
{
    [Table("rolemenurelation")]
    public class RoleMenuRelation
    {
        [Key]
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string MenuGuid { get; set; }
        public int Role { get; set; }
        public int IsDel { get; set; }
    }
}
