using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouDal;
using YouModel.DataModel;
using YouModel.ViewModel;
using YouCommon;
using YouModel;

namespace YouBll
{
    public class OrderInfoBll
    {
        private OrderInfoDal _orderInfoDal;
        private OrderProductInfoDal _orderProductInfoDal;
        private OrderProductRelationDal _orderRelationDal;
        public OrderInfoBll()
        {
            _orderProductInfoDal = new OrderProductInfoDal();
            _orderRelationDal = new OrderProductRelationDal();
            _orderInfoDal = new OrderInfoDal();
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfoVm> QueryList(int currentPage, out int totalCount)
        {
            var receipts = _orderInfoDal.GetPageData(currentPage, 10, out totalCount, "where 1=1");
            var receiptVms = CommonFunction.ConvertModel<IEnumerable<OrderInfo>, IEnumerable<OrderInfoVm>>(receipts);
            foreach (var item in receiptVms)
            {
                var orderRelations = _orderRelationDal.GetData("where OrderGuid=@OrderGuid", new { OrderGuid = item.RowGuid });
                foreach (var subItem in orderRelations)
                {
                    var orderProduct = _orderProductInfoDal.GetSingle(subItem.OrderProductInfoGuid);
                    if (null != orderProduct)
                    {
                        item.Items.Add(YouCommon.CommonFunction.ConvertModel<OrderProductInfo, OrderProductInfoVm>(orderProduct));
                    }
                }
                if (item.Items.Count > 0 && item.Items.All(o => o.Status == 1))
                {
                    item.Status = 1;
                    item.DeliveryStatus = 1;
                    var model = CommonFunction.ConvertModel<OrderInfoVm, OrderInfo>(item);
                    _orderInfoDal.Update(model);
                }
                else
                {
                    item.Status = 0;
                    var model = CommonFunction.ConvertModel<OrderInfoVm, OrderInfo>(item);
                    _orderInfoDal.Update(model);
                }
                if (item.Status != 1)
                {
                    var firstItem = item.Items.Where(o => o.Status != 1).OrderBy(o => o.DeliveryDate).FirstOrDefault();
                    if (firstItem != null)
                    {
                        var ts = DateTime.Parse(firstItem.DeliveryDate) - DateTime.Now;
                        if (ts.TotalDays <= 0)
                        {
                            item.DeliveryStatus = 4;
                        }
                        else if (ts.TotalDays > 0 && ts.TotalDays <= 7)
                        {
                            item.DeliveryStatus = 3;
                        }
                        else if (ts.TotalDays > 7)
                        {
                            item.DeliveryStatus = 2;
                        }
                    }
                    else
                    {
                        item.DeliveryStatus = 0;
                    }
                }
            }

            return receiptVms.OrderByDescending(o => o.DeliveryStatus);
        }

        public IEnumerable<OrderProductInfoVm> QueryProductList(OrderProductSearchVm search, out int totalCount)
        {
            totalCount = 0;
            var result = new List<OrderProductInfoVm>();
            var orderInfo = _orderInfoDal.GetData("where SerialId=@SerialId limit 1", new { SerialId = search.SerialId }).FirstOrDefault();
            if (null == orderInfo)
            {
                return result;
            }
            var orderRelations = _orderRelationDal.GetPageData(search.from, search.size, out totalCount, "where OrderGuid=@OrderGuid", new { OrderGuid = orderInfo.RowGuid });
            foreach (var subItem in orderRelations)
            {
                var orderProduct = _orderProductInfoDal.GetSingle(subItem.OrderProductInfoGuid);
                if (null != orderProduct)
                {
                    result.Add(YouCommon.CommonFunction.ConvertModel<OrderProductInfo, OrderProductInfoVm>(orderProduct));
                }
            }
            return result;
        }

        public bool InsertProductInfo(InsertProductInfoVm entity)
        {
            var orderInfos = _orderInfoDal.GetData("where SerialId =@SerialId limit 1", new { SerialId = entity.SerialId });
            if (orderInfos.Any())
            {
                DateTime deliveryDate;
                if (!DateTime.TryParse(entity.DeliveryDate, out deliveryDate))
                {
                    deliveryDate = DateTime.Now;
                }
                var orderInfo = orderInfos.First();
                var productInfo = new OrderProductInfo()
                {
                    CreateTime = DateTime.Now,
                    DeliveryDate = deliveryDate,
                    IsDel = 0,
                    Material = entity.Material,
                    Number = entity.Number,
                    Remark = entity.Remark,
                    RowGuid = Guid.NewGuid().ToString()
                };
                var relation = new OrderProductRelation()
                {
                    CreateTime = DateTime.Now,
                    IsDel = 0,
                    Number = 0,
                    OrderGuid = orderInfo.RowGuid,
                    OrderProductInfoGuid = productInfo.RowGuid,
                    RowGuid = Guid.NewGuid().ToString(),
                    Status = 0
                };
                _orderRelationDal.Insert(relation);
                _orderProductInfoDal.Insert(productInfo);
            }
            return true;
        }

        public bool InsertOrderInfos(string[][] orderInfos, UserInfoVm user)
        {
            Dictionary<string, OrderInfo> dicOrderInfo = new Dictionary<string, OrderInfo>();
            for (int i = 0; i < orderInfos.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                DateTime deliveryDate;
                if (!DateTime.TryParse(orderInfos[i][3].Replace('/', '-'),out deliveryDate))
                {
                    deliveryDate = DateTime.MinValue;
                }

                var orderInfo = new OrderInfo()
                {
                    CreateTime = DateTime.Now,
                    Creator = user.UserName,
                    SerialId = orderInfos[i][0],
                    Modifier = user.UserName,
                    ModifyTime = DateTime.Now,
                    RowGuid= Guid.NewGuid().ToString(),
                };
                OrderInfo tempOrderInfo;
                if (dicOrderInfo.ContainsKey(orderInfo.SerialId))
                {
                    tempOrderInfo = dicOrderInfo[orderInfo.SerialId];
                }
                else
                {
                    dicOrderInfo.Add(orderInfo.SerialId, orderInfo);
                    tempOrderInfo = orderInfo;
                    _orderInfoDal.Insert(orderInfo);
                }
                var productInfo = new OrderProductInfo()
                {
                    ProjectSerialId= orderInfos[i][1],
                    CreateTime = DateTime.Now,
                    DeliveryDate = deliveryDate,
                    IsDel = 0,
                    Material = orderInfos[i][4],
                    Number = int.Parse(orderInfos[i][8]),
                    Remark = $"{orderInfos[0][5]}:{orderInfos[i][5]},{orderInfos[0][6]}:{orderInfos[i][6]},{orderInfos[0][7]}:{orderInfos[i][7]},{orderInfos[0][9]}:{orderInfos[i][9]},{orderInfos[0][10]}:{orderInfos[i][10]},{orderInfos[0][11]}:{orderInfos[i][11]},{orderInfos[0][12]}:{orderInfos[i][12]},{orderInfos[0][13]}:{orderInfos[i][13]},{orderInfos[0][14]}:{orderInfos[i][14]}",
                    RowGuid = Guid.NewGuid().ToString()
                };
                var relation = new OrderProductRelation()
                {
                    CreateTime = DateTime.Now,
                    IsDel = 0,
                    Number = 0,
                    OrderGuid = tempOrderInfo.RowGuid,
                    OrderProductInfoGuid = productInfo.RowGuid,
                    RowGuid = Guid.NewGuid().ToString(),
                    Status = 0
                };
                _orderRelationDal.Insert(relation);
                _orderProductInfoDal.Insert(productInfo);
            }
            return true;
        }
    }
}
