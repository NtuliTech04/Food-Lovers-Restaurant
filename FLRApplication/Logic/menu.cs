using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FLRApplication.Models;
using FLRApplication.Utilities;

namespace FLRApplication.Logic
{
    public class menu
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public BinaryUpload upload = new BinaryUpload();


        public IEnumerable<MenuItem> AllItems()
        {
            return db.MenuItems.Include(i => i.Category).ToList();  
        }

        public IEnumerable<MenuItem> MenuItems()
        {
            return db.MenuItems.Where(x=>x.OnSpecial == false)
                               .Include(i => i.Category)
                               .ToList();
        }

        public IEnumerable<MenuItem> OnPromotion()
        {
            return db.MenuItems.Where(x => x.OnSpecial == true)
                               .Include(i => i.Category)
                               .ToList();
        }

        public MenuItem GetItem(int id)
        {
            return db.MenuItems.Find(id);
        }

        public bool AddItem(HttpPostedFileBase img, MenuItem model)
        {
            model.ItemImage = upload.ConvertToBytes(img);
            try
            {
                db.MenuItems.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }


        public bool NewSpecial(int id, MenuItem model)
        {
            var item = db.MenuItems.Find(id);

            try
            {
                item.Price = model.Price;
                item.PromoDesc = model.PromoDesc;
                if (model.PromoStatus == "Add Promotion")
                {
                    item.OnSpecial = true;
                }
                else
                {
                    item.OnSpecial = false;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }


        public bool EditItem(HttpPostedFileBase img, MenuItem model)
        {
            model.ItemImage = upload.ConvertToBytes(img);
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool DeleteItem(int id)
        {
            try
            {
                MenuItem menuItem = db.MenuItems.Find(id);
                db.MenuItems.Remove(menuItem);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public byte[] ItemImage(int id)
        {
            var getBinary = db.MenuItems
                              .Where(x => x.ItemID == id)
                              .Select(x => x.ItemImage);

            byte[] imageBytes = getBinary.First();

            return imageBytes;
        }
    }
}