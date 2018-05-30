using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YouModel.DataModel
{
    [Table("receiptinfo")]
    public class ReceiptInfo
    {
        public ReceiptInfo() {
            CreateTime = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public int ReceiptNumber { get; set; }
        public string ReceiptCompany { get; set; }

        public string DeliveryMan { get; set; }

        public string ReceiptMan { get; set; }

        public DateTime DelieryTime { get; set; }

        public string FileGuid { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
