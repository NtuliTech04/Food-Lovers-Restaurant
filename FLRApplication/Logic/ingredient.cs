using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FLRApplication.Models;
using FLRApplication.Utilities;

namespace FLRApplication.Logic
{
    public class ingredient
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public BinaryUpload upload = new BinaryUpload();

        public IEnumerable<Ingredient> GetIngredients(int id)
        {
            List<Ingredient> ingredList = db.Ingredients
                                             .Where(x => x.ItemID == id)
                                             .ToList();
            return ingredList;
        }

        public Ingredient Menuitem(int Id)
        {
            var item = db.Ingredients
                         .Where(x => x.ItemID == Id)
                         .Include(x => x.MenuItem)
                         .FirstOrDefault();
            return item;
        }

        public Ingredient IngredDet(int id)
        {
            return db.Ingredients.Find(id);
        }


        public bool AddIngredient(HttpPostedFileBase img, Ingredient model)
        {
            model.IngredImage = upload.ConvertToBytes(img);

            if (model.SoldIn == "Quantity")
            {
                model.InitAmount = "1";
            }
            else
            {
                model.InitAmount = "Regular";
            }

            try
            {
                db.Ingredients.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool EditIngred(HttpPostedFileBase img, Ingredient model)
        {
            model.IngredImage = upload.ConvertToBytes(img);

            if (model.SoldIn == "Quantity")
            {
                model.InitAmount = "1";
            }
            else
            {
                model.InitAmount = "Regular";
            }

            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool DeleteIngred(int id)
        {
            try
            {
                Ingredient ingredient = db.Ingredients.Find(id);
                db.Ingredients.Remove(ingredient);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }



        public byte[] IngredImage(int Id)
        {
            var getBinary = db.Ingredients
                              .Where(x => x.IngredID == Id)
                              .Select(x => x.IngredImage);

            byte[] imageBytes = getBinary.First();

            return imageBytes;
        }
    }
}