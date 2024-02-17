using System.Web;
using System.Web.Mvc;
using FLRApplication.Utilities;
using FLRApplication.Models;
using FLRApplication.Logic;

namespace FLRApplication.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public BinaryUpload upload = new BinaryUpload();
        public menu mn = new menu();
        public category ct = new category();


        public ActionResult Index()
        {
            return View(mn.AllItems());
        }
        
        public ActionResult MenuItemDetails(int id)
        {
            return View(mn.GetItem(id));
        }

        [HttpGet]
        public ActionResult AddMenuItem()
        {
            ViewBag.CategID = new SelectList(db.Categories, "CategID", "CategName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMenuItem(MenuItem model)
        {
            ViewBag.CategID = new SelectList(ct.AllCategories(), "CategID", "CategName");
            
            HttpPostedFileBase image = Request.Files["MenuItem"];

            bool Saved = mn.AddItem(image, model);

            if (Saved == true)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult PromoteItem(int? id)
        {
            MenuItem special = db.MenuItems.Find(id);

            if (special == null)
            {
                return HttpNotFound();
            }

            return View("PromoteItem", special);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoteItem(int id, MenuItem model)
        {
            bool special = mn.NewSpecial(id, model);

            if(special == true)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditMenuItem(int? id)
        {
            ViewBag.CategID = new SelectList(db.Categories, "CategID", "CategName");

            MenuItem menuItem = db.MenuItems.Find(id);

            if (menuItem == null)
            {
                return HttpNotFound();
            }

            return View("EditMenuItem", menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenuItem(MenuItem model)
        {
            ViewBag.CategID = new SelectList(ct.AllCategories(), "CategID", "CategName");

            HttpPostedFileBase image = Request.Files["MenuItem"];

            bool edit = mn.EditItem(image, model);

            if (edit == true)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult DeleteMenuItem(int? id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View("DeleteMenuItem", menuItem);
        }

        [HttpPost, ActionName("DeleteMenuItem")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool delete = mn.DeleteItem(id);
            if(delete == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult RetrieveImage(int id)
        {
            byte[] imageBytes = mn.ItemImage(id);

            if (imageBytes != null)
            {
                return File(imageBytes, "image/jPG");
            }
            else
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}