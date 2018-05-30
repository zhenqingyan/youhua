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
    public class ProductBll
    {
        private ProductInfoDal _productInfoDal;

        public ProductBll()
        {
            _productInfoDal = new ProductInfoDal();
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<ProductInfoVm> QueryList(int currentPage, out int totalCount)
        {
            var productInfo = _productInfoDal.GetPageData(currentPage, 10, out totalCount, "where 1=1");
            var result = CommonFunction.ConvertModel<IEnumerable<ProductInfo>, IEnumerable<ProductInfoVm>>(productInfo);
            return result;
        }

        public bool Insert(ProductInfoVm entity)
        {
            var model = CommonFunction.ConvertModel<ProductInfoVm, ProductInfo>(entity);
            model.RowGuid = Guid.NewGuid().ToString();
            model.Remark = model.Remark ?? string.Empty;
            model.CreateTime = DateTime.Now;
            return _productInfoDal.Insert(model);
        }
    }
}
