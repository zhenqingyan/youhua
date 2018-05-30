using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouDal;
using YouModel;
using Newtonsoft.Json;
using YouCommon;
using YouModel.DataModel;

namespace YouBll
{
    public class ReceiptInfoBll
    {
        private ReceiptInfoDal _receiptInfoDal;
        private ReceiptProductDal _receiptProductDal;
        public ReceiptInfoBll() {

            _receiptInfoDal = new ReceiptInfoDal();
            _receiptProductDal = new ReceiptProductDal();
        }

        public void Insert(ReceiptInfoVm entity)
        {
            var receiptInfo = CommonFunction.ConvertModel<ReceiptInfoVm, ReceiptInfo>(entity);
            receiptInfo.RowGuid = Guid.NewGuid().ToString();
            receiptInfo.CreateTime = DateTime.Now;
            var receiptItems =CommonFunction.ConvertModel<List<ReceiptProductVm>, List<ReceiptProduct>>(entity.Items.Where(o=>o.Status==1).ToList());

            _receiptInfoDal.Insert(receiptInfo);

            foreach (var item in receiptItems)
            {
                item.Remark = string.Empty;
                item.RowGuid = Guid.NewGuid().ToString();
                item.ReceiptGuid = receiptInfo.RowGuid;
                item.CreateTime = DateTime.Now;
                _receiptProductDal.Insert(item);
            }
        }

        public IEnumerable<ReceiptInfoVm> QueryList(int currentPage, out int totalCount)
        {
            var receipts = _receiptInfoDal.GetPageData(currentPage, 10, out totalCount, "where 1=1");
            List<string> receiptGuid = receipts.Select(o => o.RowGuid).ToList();
            var products = _receiptProductDal.GetData("where ReceiptGuid in @ReceiptGuids", new { ReceiptGuids = receiptGuid });

            var receiptVms= CommonFunction.ConvertModel<IEnumerable<ReceiptInfo>, IEnumerable<ReceiptInfoVm>>(receipts);
            foreach (var item in receiptVms)
            {
                var productVms = CommonFunction.ConvertModel<List<ReceiptProduct>, List<ReceiptProductVm>>(products.Where(o => o.ReceiptGuid == item.RowGuid).ToList());
                item.Items.AddRange(productVms);
            }
            return receiptVms;
        }

    }
}
