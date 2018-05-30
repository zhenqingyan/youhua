using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YouModel.DataModel
{
    [Table("orderinfo")]
    public class OrderInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
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
        public string ProjectNo { get; set; } = string.Empty;
        /// <summary>
        /// 凭证日期
        /// </summary>
        public DateTime VoucherDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 物料
        /// </summary>
        public string Materiel { get; set; } = string.Empty;
        /// <summary>
        /// 图号
        /// </summary>
        public string FigureNo { get; set; } = string.Empty;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = string.Empty;
        /// <summary>
        /// 短文本
        /// </summary>
        public string ShortText { get; set; } = string.Empty;
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; } = 1;
        /// <summary>
        /// 变式
        /// </summary>
        public string VariableType { get; set; } = string.Empty;
        /// <summary>
        /// 变式公式
        /// </summary>
        public string VariableFormula { get; set; } = string.Empty;
        /// <summary>
        /// 检测控制计划
        /// </summary>
        public string TestingPlan { get; set; } = string.Empty;
        /// <summary>
        /// 是否开票
        /// </summary>
        public string IsTicket { get; set; } = string.Empty;
        /// <summary>
        /// 友华交期
        /// </summary>
        public DateTime YouhuaDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; } = string.Empty;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; } = string.Empty;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;
    }
}
