using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.ViewModel
{
    public class SheetInfoVm
    {

        public SheetInfoVm()
        {
            CreateTime = DateTime.Now.ToString();
            Items = new List<SheetProductVm>();
        }
        public int Id { get; set; }

        public string RowGuid { get; set; }
        public string OrderProductGuid { get; set; }
        public string SheetNo { get; set; }
        /// <summary>
        /// 状态 0：未审核，1：审核通过
        /// </summary>
        public int Status { get; set; }
        public int Number { get; set; }

        public int MadeNumber { get; set; }
        public string Creator { get; set; }
        public string CreateTime { get; set; }
        public List<SheetProductVm> Items { get; set; }
    }
}
