using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1611154_NguyenDatMinh_ASP_Lab05.Controllers
{
    public class SportShopController : Controller
    {
        SportShopDBDataContext _context = new SportShopDBDataContext();
        // GET: SportShop
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if(file != null)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
                file.SaveAs(destFile);

            }
            return View();
        }

        public ActionResult ListItems()
        {
            var items = _context.SportItems.ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] SportItem item)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(item.ImagePath);

                item.ImagePath = "Images/" + fileName;
                item.DateAdd = DateTime.Now.ToString();
                item.Status = "AVAILABLE";

                _context.SportItems.InsertOnSubmit(item);
                _context.SubmitChanges();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.SportItems.ToList().Find(m => m.Id == id);
            if (item != null)
            {
                _context.SportItems.DeleteOnSubmit(item);
                _context.SubmitChanges();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id)
        {
            var item = _context.SportItems.ToList().Find(m => m.Id == id);

            string rootPath = Server.MapPath("~/");
            string fileName = System.IO.Path.GetFileName(item.ImagePath);
            string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
            item.ImagePath = destFile;

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(SportItem item)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(item.ImagePath);
                item.ImagePath = "Images/" + fileName;

                SportItem i = (from si in _context.SportItems
                                  where si.Id == item.Id
                                  select si).SingleOrDefault();

                i.SportName = item.SportName;
                i.ImagePath = item.ImagePath;
                i.Status = item.Status;
                i.Price = item.Price;
                i.Quantity = item.Quantity;
                i.Type = item.Type;

                _context.SubmitChanges();

            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}