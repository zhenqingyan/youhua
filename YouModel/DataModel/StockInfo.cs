using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YouModel.DataModel
{
    [Table("stockinfo")]
    public class StockInfo
    {
        [Key]
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string ProductInfoGuid { get; set; }
        public int Number { get; set; }
        public int UseNumber { get; set; }
        public int IsDel { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
