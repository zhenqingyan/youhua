using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;
using YouModel.ViewModel;
using YouModel;
using YouWeb.Filter;

namespace YouWeb.Controllers
{
    public class WorkSpaceController : BaseController
    {

        private WorkSpaceBll _workSpaceBll;

        public WorkSpaceController()
        {
            _workSpaceBll = new WorkSpaceBll();
        }
        [MenuAuthrity]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertPurchase(PurchaseInfoVm entity)
        {
            return Json(_workSpaceBll.InsertPurchase(entity));
        }
        [HttpPost]
        public JsonResult InsertPurchaseProduct(PurchaseProductVm entity)
        {
            return Json(_workSpaceBll.InsertPurchaseProduct(entity));
        }


        [HttpGet]
        public JsonResult QueryPurchase(string proGuid)
        {
            return Json(_workSpaceBll.QueryPurchaseInfo(proGuid), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 审核采购单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditPurchase(PurchaseInfoVm entity)
        {
            return Json(_workSpaceBll.AuditPurchase(entity));
        }
        /// <summary>
        /// 接收采购单产品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReveiveProduct(PurchaseProductVm entity)
        {
            return Json(_workSpaceBll.ReveiveProduct(entity));
        }
        /// <summary>
        /// 检验采购单产品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditProduct(PurchaseProductVm entity)
        {
            return Json(_workSpaceBll.AuditProduct(entity));
        }



        [HttpPost]
        public JsonResult InsertSheet(SheetInfoVm entity)
        {
            return Json(_workSpaceBll.InsertSheet(entity));
        }
        [HttpPost]
        public JsonResult InsertSheetProduct(SheetProductVm entity)
        {
            return Json(_workSpaceBll.InsertSheetProduct(entity));
        }
        [HttpGet]
        public JsonResult QuerySheet(string proGuid)
        {
            return Json(_workSpaceBll.QuerySheetInfo(proGuid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AuditSheet(SheetInfoVm entity)
        {
            return Json(_workSpaceBll.AuditSheet(entity));
        }
        [HttpPost]
        public JsonResult GetProduct(SheetProductVm entity)
        {
            return Json(_workSpaceBll.GetProduct(entity));
        }
        [HttpPost]
        public JsonResult MadeProduct(SheetInfoVm entity)
        {
            return Json(_workSpaceBll.MadeProduct(entity));
        }
    }
}