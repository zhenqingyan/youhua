using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouModel;
using YouBll;
using YouModel.ViewModel;
using YouWeb.Filter;
using Newtonsoft.Json;

namespace YouWeb.Controllers
{
  
    public class OrderInfoController : BaseController
    {
        private OrderInfoBll _orderInfoBll;
        public OrderInfoController()
        {
            _orderInfoBll = new OrderInfoBll();
        }
        [MenuAuthrity]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetData(SearchVm search)
        {
            int totalCount;
            var result = _orderInfoBll.QueryList(search.from, out totalCount);
            return Json(new
            {
                count = totalCount,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductData(OrderProductSearchVm search)
        {
            int totalCount;
            var result = _orderInfoBll.QueryProductList(search, out totalCount);
            return Json(new
            {
                count = totalCount,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertProductInfo(InsertProductInfoVm entity)
        {
            return Json(_orderInfoBll.InsertProductInfo(entity));
        }

        [HttpPost]
        public JsonResult InsertOrders(string[][] orderInfos)
        {
            return Json(_orderInfoBll.InsertOrderInfos(orderInfos, base.UserInfo));
        }
    }
}