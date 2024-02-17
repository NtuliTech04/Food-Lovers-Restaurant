using System.Linq;
using System.Net;
using System.Web.Mvc;
using FLRApplication.Logic;
using FLRApplication.Models;

namespace FLRApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public category ct = new category();


        public ActionResult Index()
        {
            return View(ct.AllCategories());
        }

        public ActionResult ViewCategory(int id)
        {
            return View(ct.CategoryDetails(id));
        }

        [HttpGet]
        public ActionResult NewCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCategory(Category model)
        {
            bool save = ct.AddCategory(model);

            if (save == true)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditCategory(int? id )
        {
            var category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View("EditCategory", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Category model)
        {
            bool edit = ct.EditCategory(model);

            if (edit == true)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult DeleteCategory(int? id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View("DeleteCategory", category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool delete = ct.DeleteCategory(id);

            if (delete == true)
            {
                return RedirectToAction("Index");
            }
            return View();
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