using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.ViewModel
{
    public class PurchaseInfoVm
    {

        public PurchaseInfoVm()
        {
            CreateTime = DateTime.Now.ToString();
            Items = new List<PurchaseProductVm>();
        }
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string OrderProductGuid { get; set; }
        public string PurchaseNo { get; set; }
        /// <summary>
        /// 状态 0：未审核，1：审核通过
        /// </summary>
        public int Status { get; set; }
        public string Creator { get; set; }
        public string CreateTime { get; set; }
        public List<PurchaseProductVm> Items { get; set; }
    }
}
