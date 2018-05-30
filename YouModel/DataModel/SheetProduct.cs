using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YouModel.DataModel
{
    [Table("sheetProduct")]
    public class SheetProduct
    {
        [Key]
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string SheetGuid { get; set; }
        public string ProductInfoGuid { get; set; }
        public int Number { get; set; }
        public int GetNumber { get; set; }
        public int Status { get; set; }
        public int IsDel { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
