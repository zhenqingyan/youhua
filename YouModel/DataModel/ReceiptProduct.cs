using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel;

namespace YouModel.DataModel
{
    [Table("receiptproduct")]
    public class ReceiptProduct
    {
        public ReceiptProduct()
        {
            Remark = string.Empty;
            CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string Material { get; set; }
        public string NameAndType { get; set; }
        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseNumber { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// 
        public int Number { get; set; }
        /// <summary>
        /// 收货数量
        /// </summary>
        public int ReceiveNumber { get; set; }
        public string Weight { get; set; }
        public string TotalWeight { get; set; }
        public string FurnaceNumber { get; set; }
        public string Remark { get; set; }
        public string ReceiptGuid { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 状态 0:采购中，1：待检验 2：已入库
        /// </summary>
        public int Status { get; set; }
    }
}
