using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouBll;
using System.IO;
using YouModel;
using YouWeb.Filter;

namespace YouWeb.Controllers
{
    public class HomeController : BaseController
    {

        private DeliveryInfoBll _deliveryBll;
        private ReceiptInfoBll _receiptInfoBll;
        public HomeController()
        {
            _deliveryBll = new DeliveryInfoBll();
            _receiptInfoBll = new ReceiptInfoBll();
        }
        [MenuAuthrity]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Insert(ReceiptInfoVm viewModel)
        {
            _receiptInfoBll.Insert(viewModel);
            return Json(new { Flag = true, Msg = "成功" });
        }

        [HttpPost]
        public JsonResult GetData(SearchVm search)
        {
            int totalCount;
            var result = _receiptInfoBll.QueryList(search.from, out totalCount);
            return Json(new
            {
                count = totalCount,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpLoadPic()
        {
            var file = Request.Files[0];
            //var fileNames = file.FileName.Split('.');
            var reqFileSteam = file.InputStream;
            var fileBytes = new byte[reqFileSteam.Length];
            reqFileSteam.Read(fileBytes, 0, (int)reqFileSteam.Length);

            var FileGuid = Guid.NewGuid().ToString();
            var path=Server.MapPath("/")+"Files\\";
            var fileSteam = new FileStream($"{path}{FileGuid}.jpg", FileMode.Create);
            fileSteam.Write(fileBytes, 0, fileBytes.Length);

            fileSteam.Close();
            reqFileSteam.Close();

            return Json(new { Flag = true, FileName = FileGuid });
        }

        public FileResult GetPic(string fileName)
        {
            var path = Server.MapPath("/") + "Files\\";
            return File($"{path}{fileName}.jpg","image/jpeg");
        }
    }
}