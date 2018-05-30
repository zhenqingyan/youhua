using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel
{
    public class OrderProductInfoVm
    {
        public OrderProductInfoVm()
        {
            CreateTime = DateTime.Now.ToString();
            Remark = string.Empty;
        }
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string ProjectSerialId { get; set; }
        public string Material { get; set; }
        public int Number { get; set; }
        public int FinishNumber { get; set; }
        public string Remark { get; set; }
        public string DeliveryDate { get; set; }
        public string CreateTime { get; set; }
        public int Status { get; set; }
        public int IsDel { get; set; }
    }
}
