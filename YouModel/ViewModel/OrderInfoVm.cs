using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouModel.ViewModel
{
    public class OrderInfoVm
    {

        public OrderInfoVm()
        {
            Items = new List<OrderProductInfoVm>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 逻辑主键
        /// </summary>
        public string RowGuid { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SerialId { get; set; }
        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo { get; set; }
        /// <summary>
        /// 凭证日期
        /// </summary>
        public string VoucherDate { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public string DeliveryDate { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public string Materiel { get; set; }
        /// <summary>
        /// 图号
        /// </summary>
        public string FigureNo { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 短文本
        /// </summary>
        public string ShortText { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 变式
        /// </summary>
        public string VariableType { get; set; }
        /// <summary>
        /// 变式公式
        /// </summary>
        public string VariableFormula { get; set; }
        /// <summary>
        /// 检测控制计划
        /// </summary>
        public string TestingPlan { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public string IsTicket { get; set; }
        /// <summary>
        /// 友华交期
        /// </summary>
        public string YouhuaDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 状态
        /// 0创建 1采购中 2收货中 3收货检验中 4入库完成 5工艺流转中 6提货
        /// </summary>
        public int Status { get; set; }

        public IList<OrderProductInfoVm> Items { get; set; }

        /// <summary>
        /// 4非常紧急 3紧急 2加紧 1已完成 0正常
        /// </summary>
        public int DeliveryStatus { get; set; }
    }
}
