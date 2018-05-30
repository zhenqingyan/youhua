using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.DataModel
{
    [Table("purchaseInfo")]
    public class PurchaseInfo
    {
        [Key]
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string OrderProductGuid { get; set; }
        public string PurchaseNo { get; set; }

        /// <summary>
        /// 状态 0：未审核，1：审核通过
        /// </summary>
        public int Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsDel { get; set; }
    }
}
