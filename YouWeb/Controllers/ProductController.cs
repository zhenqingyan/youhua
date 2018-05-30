using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;
using YouModel;
using YouWeb.Filter;

namespace YouWeb.Controllers
{
    public class ProductController : BaseController
    {

        private ProductBll _productBll;
        public ProductController()
        {
            _productBll = new ProductBll();
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
            var result = _productBll.QueryList(search.from, out totalCount);
            return Json(new
            {
                count = totalCount,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert(ProductInfoVm entity)
        {
            return Json(_productBll.Insert(entity));
        }

    }
}