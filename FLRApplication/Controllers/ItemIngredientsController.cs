using System.Web;
using System.Web.Mvc;
using FLRApplication.Utilities;
using FLRApplication.Models;
using FLRApplication.ViewModels;
using FLRApplication.Logic;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Infrastructure;

namespace FLRApplication.Controllers
{
    public class ItemIngredientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ingredient ig = new ingredient();
        public menu mn = new menu();
        public BinaryUpload upload = new BinaryUpload();


        [HttpGet]
        public ActionResult ItemIngredientVM(int id, MenuIngredientVM VModel)
        {
            VModel.Ingredient = new Ingredient() { ItemID = id };
            VModel.MenuItem = mn.GetItem(id);
            VModel.ListIngredients = ig.GetIngredients(id);

            return View(VModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemIngredientVM(MenuIngredientVM VModel)
        {
            HttpPostedFileBase image = Request.Files["Ingred"];

            bool Save = ig.AddIngredient(image, VModel.Ingredient);

            if (Save == true)
            {
                return RedirectToAction("ItemIngredientVM", new { Id = VModel.Ingredient.ItemID });
            }
            return View(VModel);
        }

        public ActionResult ItemIngredients(int id)
        {
            return View(ig.GetIngredients(id));
        }

        public ActionResult IngredientDetails(int id)
        {
            return View(ig.IngredDet(id));
        }
        
        [HttpGet]
        public ActionResult EditIngredient(int? id)
        {
            Ingredient ingredient = db.Ingredients.Find(id);

            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View("EditIngredient", ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIngredient(Ingredient model)
        {
            HttpPostedFileBase image = Request.Files["Ingred"];

            bool edit = ig.EditIngred(image, model);

            if(edit == true)
            {
                return RedirectToAction("ItemIngredientVM", new { id = model.ItemID });
            }
            return View(model);
        }

        public ActionResult DeleteIngredient(int? id)
        {
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View("DeleteIngredient", ingredient);
        }

        [HttpPost, ActionName("DeleteIngredient")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool delete = ig.DeleteIngred(id);
            if (delete == true)
            {
                return RedirectToAction("Index","MenuItems");
            }
            return View();
        }


        public ActionResult RetrieveImage(int id)
        {
            byte[] imageBytes = ig.IngredImage(id);

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