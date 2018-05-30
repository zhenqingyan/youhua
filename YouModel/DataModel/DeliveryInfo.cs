using System;
using Dapper;

namespace YouModel.DataModel
{
    [Table("deliveryinfo")]
    public class DeliveryInfo
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
        /// 收获公司
        /// </summary>
        public string DeliveryCompany { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public int DeliveryNumber { get; set; }

        /// <summary>
        /// 收货经办人
        /// </summary>
        public string DeliveryMan { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileGuid { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
