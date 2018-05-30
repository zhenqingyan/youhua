using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouModel.ViewModel;
using YouDal;
using YouModel;
using YouCommon;
using YouModel.DataModel;

namespace YouBll
{
    public class WorkSpaceBll
    {
        private PurchaseInfoDal _purchaseInfoDal;
        private PurchaseProductDal _purchaseProductDal;
        private ReceiptProductDal _receiptProductDal;
        private ProductInfoDal _productInfoDal;
        private SheetInfoDal _sheetInfoDal;
        private SheetProductDal _sheetProductDal;

        private StockInfoDal _stockInfoDal;

        private OrderProductInfoDal _orderProductInfoDal;
        public WorkSpaceBll()
        {
            _purchaseInfoDal = new PurchaseInfoDal();
            _purchaseProductDal = new PurchaseProductDal();
            _receiptProductDal = new ReceiptProductDal();
            _productInfoDal = new ProductInfoDal();
            _stockInfoDal = new StockInfoDal();
            _sheetInfoDal = new SheetInfoDal();
            _sheetProductDal = new SheetProductDal();
            _orderProductInfoDal = new OrderProductInfoDal();
        }
        public string ReveiveProduct(PurchaseProductVm entity)
        {
            var model = _purchaseProductDal.GetSingle(entity.RowGuid);
            if ((model.ReceiveNumber + entity.ReceiveNumber) > entity.Number)
            {
                return "超过收货最大数量";
            }
            else
            {
                model.ReceiveNumber += entity.ReceiveNumber;
                if (model.ReceiveNumber >= entity.Number)
                {
                    model.Status = 1;
                }
                return _purchaseProductDal.Update(model) ? "收货成功" : "收货失败";
            }
        }
        public string AuditProduct(PurchaseProductVm entity)
        {
            var model = _purchaseProductDal.GetSingle(entity.RowGuid);
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
                if (_purchaseProductDal.Update(model))
                {
                    _stockInfoDal.Insert(new StockInfo() {
                        CreateTime=DateTime.Now,
                        Number=model.AuditNumber,
                        ProductInfoGuid=model.ProductInfoGuid,
                        RowGuid=Guid.NewGuid().ToString(),
                    });
                    return "检验成功";
                }
                else
                {
                    return "检验失败";
                }
            }
        }
        public bool AuditPurchase(PurchaseInfoVm entity)
        {
            var model = CommonFunction.ConvertModel<PurchaseInfoVm, PurchaseInfo>(entity);
            model.Status = 1;
            return _purchaseInfoDal.Update(model);
        }
        public bool InsertPurchaseProduct(PurchaseProductVm entity)
        {
            var purchaseProduct = new PurchaseProduct()
            {
                CreateTime = DateTime.Now,
                ProductInfoGuid = entity.ProductInfoGuid,
                PurchaseGuid = entity.PurchaseGuid,
                RowGuid = Guid.NewGuid().ToString(),
                Number=entity.Number
            };
            return _purchaseProductDal.Insert(purchaseProduct);
        }

        public bool InsertPurchase(PurchaseInfoVm entity)
        {
            var purchaseInfo = new PurchaseInfo()
            {
                OrderProductGuid = entity.OrderProductGuid,
                CreateTime = DateTime.Now,
                Creator = "系统",
                PurchaseNo = entity.PurchaseNo,
                RowGuid = Guid.NewGuid().ToString(),
                IsDel = 0
            };
            return _purchaseInfoDal.Insert(purchaseInfo);
        }

        public IEnumerable<PurchaseInfoVm> QueryPurchaseInfo(string proGuid)
        {
            var purchaseInfos = _purchaseInfoDal.GetData("where OrderProductGuid=@OrderProductGuid and IsDel=0 limit 100", new
            {
                OrderProductGuid = proGuid
            });
            var purchaseInfoVms = CommonFunction.ConvertModel<IEnumerable<PurchaseInfo>, IEnumerable<PurchaseInfoVm>>(purchaseInfos);

            foreach (var item in purchaseInfoVms)
            {
                var purchaseList = _purchaseProductDal.GetData("where PurchaseGuid=@PurchaseGuid and IsDel=0", new
                {
                    PurchaseGuid = item.RowGuid
                });
                foreach (var subItem in purchaseList)
                {
                    var product = _productInfoDal.GetSingle(subItem.ProductInfoGuid);
                    item.Items.Add(new PurchaseProductVm()
                    {
                        Id = subItem.Id,
                        CreateTime = subItem.CreateTime.ToString("yyyy-MM-dd hh:mm"),
                        FurnaceNumber = product.FurnaceNumber,
                        Material = product.Material,
                        NameAndType = product.NameAndType,
                        RowGuid = subItem.RowGuid,
                        TotalWeight = product.TotalWeight,
                        Weight = product.Weight,
                        Number = subItem.Number,
                        ReceiveNumber = subItem.ReceiveNumber,
                        AuditNumber=subItem.AuditNumber,
                        Status=subItem.Status
                    });
                }
            }
            return purchaseInfoVms;
        }

        public bool AuditSheet(SheetInfoVm entity)
        {
            var model = CommonFunction.ConvertModel<SheetInfoVm, SheetInfo>(entity);
            model.Status = 1;
            return _sheetInfoDal.Update(model);
        }
        public bool InsertSheetProduct(SheetProductVm entity)
        {
            var product = new SheetProduct()
            {
                CreateTime = DateTime.Now,
                ProductInfoGuid = entity.ProductInfoGuid,
                SheetGuid = entity.SheetGuid,
                RowGuid = Guid.NewGuid().ToString(),
                Number = entity.Number
            };
            return _sheetProductDal.Insert(product);
        }
        public bool InsertSheet(SheetInfoVm entity)
        {
            var sheetInfo = new SheetInfo()
            {
                OrderProductGuid = entity.OrderProductGuid,
                CreateTime = DateTime.Now,
                Creator = "系统",
                SheetNo = entity.SheetNo,
                Number=entity.Number,
                RowGuid = Guid.NewGuid().ToString(),
                IsDel = 0
            };
            return _sheetInfoDal.Insert(sheetInfo);
        }
        public IEnumerable<SheetInfoVm> QuerySheetInfo(string proGuid)
        {
            var Infos = _sheetInfoDal.GetData("where OrderProductGuid=@OrderProductGuid and IsDel=0 limit 100", new
            {
                OrderProductGuid = proGuid
            });
            var InfoVms = CommonFunction.ConvertModel<IEnumerable<SheetInfo>, IEnumerable<SheetInfoVm>>(Infos);

            foreach (var item in InfoVms)
            {
                var list = _sheetProductDal.GetData("where SheetGuid=@SheetGuid and IsDel=0", new
                {
                    SheetGuid = item.RowGuid
                });
                foreach (var subItem in list)
                {
                    var product = _productInfoDal.GetSingle(subItem.ProductInfoGuid);
                    var stockinfo = _stockInfoDal.GetData("where ProductInfoGuid=@ProductInfoGuid and IsDel=0", new { ProductInfoGuid = subItem.ProductInfoGuid });
                    var canGetNumber = stockinfo.Sum(o => o.Number - o.UseNumber);
                    item.Items.Add(new SheetProductVm()
                    {
                        Id = subItem.Id,
                        CreateTime = subItem.CreateTime.ToString("yyyy-MM-dd hh:mm"),
                        FurnaceNumber = product.FurnaceNumber,
                        Material = product.Material,
                        NameAndType = product.NameAndType,
                        RowGuid = subItem.RowGuid,
                        TotalWeight = product.TotalWeight,
                        Weight = product.Weight,
                        Number = subItem.Number,
                        GetNumber=subItem.GetNumber,
                        Status = subItem.Status,
                        StockNumber= canGetNumber,
                        ProductInfoGuid=subItem.ProductInfoGuid,
                        SheetGuid=subItem.SheetGuid
                    });
                }
            }
            return InfoVms;
        }
        public string GetProduct(SheetProductVm entity)
        {
            var model = _sheetProductDal.GetSingle(entity.RowGuid);
          
            if ((model.GetNumber + entity.GetNumber) > entity.Number)
            {
                return "超过提货最大数量";
            }
            else
            {
                var stockinfo = _stockInfoDal.GetData("where ProductInfoGuid=@ProductInfoGuid and IsDel=0", new { ProductInfoGuid = entity.ProductInfoGuid });
                var canGetNumber = stockinfo.Sum(o => o.Number - o.UseNumber);
                if (entity.GetNumber > canGetNumber)
                {
                    return "没有足够库存";
                }
                model.GetNumber += entity.GetNumber;
                if (model.GetNumber >= entity.Number)
                {
                    model.Status = 1;
                }
                if (_sheetProductDal.Update(model))
                {
                    var getNumber = entity.GetNumber;
                    foreach (var item in stockinfo)
                    {
                        if (getNumber <= 0)
                        {
                            break;
                        }
                        if ((item.Number - item.UseNumber) >= getNumber)
                        {
                            item.UseNumber += getNumber;
                            getNumber = 0;
                        }
                        else
                        {
                            getNumber -= (item.Number - item.UseNumber);
                            item.UseNumber = item.Number;
                        }
                        _stockInfoDal.Update(item);
                    }
                    var sheetProducts = _sheetProductDal.GetData("where SheetGuid=@SheetGuid", new { SheetGuid = model.SheetGuid });
                    var allNumbers=sheetProducts.Sum(o => o.Number - o.GetNumber);
                    if (allNumbers==0)
                    {
                        var sheetInfo=_sheetInfoDal.GetSingle(model.SheetGuid);
                        sheetInfo.Status = 1;
                        _sheetInfoDal.Update(sheetInfo);
                    }
                    return "提货成功";
                }
                else
                {
                    return "提货失败";
                }
            }
        }

        public string MadeProduct(SheetInfoVm entity)
        {
            var model = _sheetInfoDal.GetSingle(entity.RowGuid);
            if ((model.MadeNumber + entity.MadeNumber) > model.Number)
            {
                return "超过入库最大数量";
            }
            model.MadeNumber += entity.MadeNumber;
            if (model.MadeNumber >= model.Number)
            {
                model.Status = 2;
            }
            if (_sheetInfoDal.Update(model))
            {
                var orderProduct = _orderProductInfoDal.GetSingle(model.OrderProductGuid);
                var sheetInfos=_sheetInfoDal.GetData("where OrderProductGuid=@OrderProductGuid",new { OrderProductGuid =model.OrderProductGuid});
                if (sheetInfos.Sum(o => o.MadeNumber) == orderProduct.Number)
                {
                    orderProduct.Status = 1;
                    _orderProductInfoDal.Update(orderProduct);
                }
              
                return "提货成功";
            }
            else
            {
                return "提货失败";
            }
        }
    }
}
