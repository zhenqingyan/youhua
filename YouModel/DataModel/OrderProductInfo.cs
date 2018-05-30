using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel;

namespace YouModel.DataModel
{
    [Table("orderproductinfo")]
    public class OrderProductInfo
    {
        public OrderProductInfo()
        {
            Remark = string.Empty;
            CreateTime = DateTime.Now;
            DeliveryDate = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string ProjectSerialId { get; set; }
        public string Material { get; set; }
        public int Number { get; set; }
        public int FinishNumber { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsDel { get; set; }
    }
}
