using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel;

namespace YouModel.DataModel
{
    [Table("productInfo")]
    public class ProductInfo
    {
        public ProductInfo()
        {
            Remark = string.Empty;
            CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string Material { get; set; }
        public string NameAndType { get; set; }
        public string Weight { get; set; }
        public string TotalWeight { get; set; }
        public string FurnaceNumber { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsDel { get; set; }
    }
}
