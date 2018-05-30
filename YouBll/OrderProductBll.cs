using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCommon;
using YouDal;
using YouModel;
using YouModel.DataModel;

namespace YouBll
{
    public class OrderProductBll
    {
        private OrderProductInfoDal _orderproductInfoDal;

        public OrderProductBll()
        {
            _orderproductInfoDal = new OrderProductInfoDal();
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<OrderProductInfoVm> QueryList(int currentPage, out int totalCount)
        {
            var productInfo = _orderproductInfoDal.GetPageData(currentPage, 10, out totalCount, "where 1=1");
            var result = CommonFunction.ConvertModel<IEnumerable<OrderProductInfo>, IEnumerable<OrderProductInfoVm>>(productInfo);
            return result;
        }

        public bool Insert(OrderProductInfoVm entity)
        {
            var model = CommonFunction.ConvertModel<OrderProductInfoVm, OrderProductInfo>(entity);
            model.RowGuid = Guid.NewGuid().ToString();
            model.Remark = model.Remark ?? string.Empty;
            model.CreateTime = DateTime.Now;
            return _orderproductInfoDal.Insert(model);
        }
    }
}
