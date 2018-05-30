using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouCommon;
using YouDal;
using YouModel.ViewModel;
using YouModel.DataModel;

namespace YouBll
{
    public class StockBll
    {
        private StockInfoDal _stockInfoDal;
        private ProductInfoDal _productInfoDal;
        public StockBll() {
            _stockInfoDal = new StockInfoDal();
            _productInfoDal = new ProductInfoDal();
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<StockInfoVm> QueryList(int currentPage, out int totalCount)
        {
            var stockInfo = _stockInfoDal.GetPageData(currentPage, 10, out totalCount, "where 1=1");


            var result = CommonFunction.ConvertModel<IEnumerable<StockInfo>, IEnumerable<StockInfoVm>>(stockInfo);
            var productInfos = _productInfoDal.GetData("where RowGuid in @productGuids",new {
                productGuids= result.Select(o=>o.ProductInfoGuid).ToList()
            });
            foreach (var item in result)
            {
                var product = productInfos.Where(o => o.RowGuid == item.ProductInfoGuid).FirstOrDefault();
                item.FurnaceNumber = product.FurnaceNumber;
                item.Material = product.Material;
                item.NameAndType = product.NameAndType;
                item.TotalWeight = product.TotalWeight;
                item.Weight = product.Weight;
            }
            return result;
        }
    }
}
