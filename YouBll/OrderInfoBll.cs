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
        private PurchaseInfoDal _purchaseInfoDal;
        private SheetInfoDal _sheetInfoDal;
        public OrderInfoBll()
        {
            _orderProductInfoDal = new OrderProductInfoDal();
            _orderRelationDal = new OrderProductRelationDal();
            _orderInfoDal = new OrderInfoDal();
            _purchaseInfoDal = new PurchaseInfoDal();
            _sheetInfoDal = new SheetInfoDal();
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
                if (item.Items.Count > 0 && item.Items.All(o => o.Status == 3))
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
                    var checkItems = item.Items.Where(o => o.Status != 3);
                    foreach (var checkItem in checkItems)
                    {
                        TimeSpan ts = DateTime.Parse(checkItem.DeliveryDate) - DateTime.Now;
                        var purchaseFlag = !_purchaseInfoDal.GetData("where OrderProductGuid=@OrderProductGuid", new { OrderProductGuid = checkItem.RowGuid }).Any();
                        if (ts.TotalDays <= 20 && purchaseFlag)
                        {
                            item.DeliveryStatus = 3;
                            break;
                        }
                        var sheetFlag = !_sheetInfoDal.GetData("where OrderProductGuid=@OrderProductGuid", new { OrderProductGuid = checkItem.RowGuid }).Any();
                        if (ts.TotalDays <= 10 && sheetFlag)
                        {
                            item.DeliveryStatus = 3;
                            break;
                        }
                        if (ts.TotalDays <= 3 && checkItem.AuditNumber != checkItem.Number)
                        {
                            item.DeliveryStatus = 3;
                            break;
                        }
                        if (purchaseFlag || sheetFlag)
                        {
                            item.DeliveryStatus = 2;
                        }
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

        public string InsertOrderInfos(string[][] orderInfos, UserInfoVm user)
        {
            var result = 0;
            int orderInfex = 0;
            int projectIndex = 0;
            int deliveryIndex = 0;
            int materialIndex = 0;
            int numberIndix = 0;
            for (int j = 0; j < orderInfos[0].Length; j++)
            {
                if (orderInfos[0][j] == "订单")
                {
                    orderInfex = j;
                }
                else if (orderInfos[0][j] == "项目")
                {
                    projectIndex = j;
                }
                else if (orderInfos[0][j] == "交货日期")
                {
                    deliveryIndex = j;
                }
                else if (orderInfos[0][j] == "物料")
                {
                    materialIndex = j;
                }
                else if (orderInfos[0][j] == "数量")
                {
                    numberIndix = j;
                }
            }
            Dictionary<string, OrderInfo> dicOrderInfo = new Dictionary<string, OrderInfo>();
            for (int i = 0; i < orderInfos.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                var orderNoStr = orderInfos[i][orderInfex];
                var projectNoStr = orderInfos[i][projectIndex];
                var deliveryDateStr = orderInfos[i][deliveryIndex];
                var materialStr = orderInfos[i][materialIndex];
                var numberStr = orderInfos[i][numberIndix];
                DateTime deliveryDate;
                var timeStr = deliveryDateStr.Split('/');
                var dateStr = "20" + timeStr[2] + "-" + timeStr[0].PadLeft(2, '0') + "-" + timeStr[1].PadLeft(2, '0');

                if (!DateTime.TryParse(dateStr, out deliveryDate))
                {
                    deliveryDate = DateTime.MinValue;
                }
                var orderInfo = new OrderInfo()
                {
                    CreateTime = DateTime.Now,
                    Creator = user.UserName,
                    SerialId = orderNoStr,
                    Modifier = user.UserName,
                    ModifyTime = DateTime.Now,
                    RowGuid = Guid.NewGuid().ToString(),
                };

                OrderInfo tempOrderInfo;
                if (dicOrderInfo.ContainsKey(orderInfo.SerialId))
                {
                    tempOrderInfo = dicOrderInfo[orderInfo.SerialId];
                }
                else
                {
                    var orderInfoVm = _orderInfoDal.GetData("where SerialId=@SerialId limit 1", new { SerialId = orderNoStr }).FirstOrDefault();
                    if (orderInfoVm == null)
                    {
                        _orderInfoDal.Insert(orderInfo);
                    }
                    else
                    {
                        orderInfo.RowGuid = orderInfoVm.RowGuid;
                    }
                    dicOrderInfo.Add(orderInfo.SerialId, orderInfo);
                    tempOrderInfo = orderInfo;
                }
                var tempProduct = _orderRelationDal.GetData("where OrderGuid=@OrderGuid and ProjectSerialid=@ProjectSerialid",
                    new { OrderGuid = tempOrderInfo.RowGuid, ProjectSerialid = projectNoStr }).FirstOrDefault();
                if (tempProduct != null)
                {
                    continue;
                }

                var productInfo = new OrderProductInfo()
                {
                    ProjectSerialId = projectNoStr,
                    CreateTime = DateTime.Now,
                    DeliveryDate = deliveryDate,
                    IsDel = 0,
                    Material = materialStr,
                    Number = int.Parse(numberStr),
                    Remark = GetRemarkInfo(i, orderInfos),
                    RowGuid = Guid.NewGuid().ToString()
                };
                var relation = new OrderProductRelation()
                {
                    CreateTime = DateTime.Now,
                    IsDel = 0,
                    Number = 0,
                    OrderGuid = tempOrderInfo.RowGuid,
                    OrderProductInfoGuid = productInfo.RowGuid,
                    ProjectSerialid = projectNoStr,
                    RowGuid = Guid.NewGuid().ToString(),
                    Status = 0
                };
                _orderRelationDal.Insert(relation);
                _orderProductInfoDal.Insert(productInfo);
                result++;
            }
            return "添加数量:" + result;
        }
        private string GetRemarkInfo(int i, string[][] data)
        {
            var sb = new StringBuilder();
            for (int j = 0; j < data[i].Length; j++)
            {
                if (data[0][j] != "订单"
                    && data[0][j] != "项目"
                    && data[0][j] != "交货日期"
                    && data[0][j] != "物料"
                    && data[0][j] != "数量")
                {
                    sb.Append($"{data[0][j]}:{data[i][j]}    ");
                }

            }
            return sb.ToString();
        }

        public string AuditProduct(OrderProductInfoVm entity)
        {
            var model = _orderProductInfoDal.GetSingle(entity.RowGuid);
            if ((model.AuditNumber + entity.AuditNumber) > entity.Number)
            {
                return "超过检验最大数量";
            }
            else
            {
                model.AuditNumber += entity.AuditNumber;
                if (model.AuditNumber >= entity.Number)
                {
                    model.Status = 2;
                }
                if (_orderProductInfoDal.Update(model))
                {
                    return "检验成功";
                }
                else
                {
                    return "检验失败";
                }
            }
        }
        public string DeliverProduct(OrderProductInfoVm entity)
        {
            var model = _orderProductInfoDal.GetSingle(entity.RowGuid);
            if (model.Status != 2)
            {
                return "还未全部检验完毕";
            }
            else
            {
                model.FinishNumber = model.Number;
                model.Status = 3;
                if (_orderProductInfoDal.Update(model))
                {
                    return "发货成功";
                }
                else
                {
                    return "发货失败";
                }
            }
        }
    }
}
