using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel
{
    public class ReceiptInfoVm
    {

        public ReceiptInfoVm() {
            CreateTime = DateTime.Now.ToString();
            Items = new List<ReceiptProductVm>();
        }
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public int ReceiptNumber { get; set; }
        public string ReceiptCompany { get; set; }

        public string DeliveryMan { get; set; }

        public string ReceiptMan { get; set; }

        public string DelieryTime { get; set; }

        public string FileGuid { get; set; }
        public string CreateTime { get; set; }
        public List<ReceiptProductVm> Items { get; set; }

    }
}
