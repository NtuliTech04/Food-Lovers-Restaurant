using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FLRApplication.Models;

namespace FLRApplication.Logic
{
    public class category
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<Category> AllCategories()
        {
            return db.Categories.ToList();
        }

        public Category CategoryDetails(int id)
        {
            return db.Categories.Find(id);
        }

        public bool AddCategory(Category model)
        {
            try
            {
                db.Categories.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool EditCategory(Category model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}