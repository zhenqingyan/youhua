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
    public class StockController : BaseController
    {
        private StockBll _stockBll;
        public StockController()
        {
            _stockBll = new StockBll();
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
            var result = _stockBll.QueryList(search.from, out totalCount);
            return Json(new
            {
                count = totalCount,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }
    }
}