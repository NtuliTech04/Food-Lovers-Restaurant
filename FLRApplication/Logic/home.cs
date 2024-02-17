using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FLRApplication.Models;
using FLRApplication.Utilities;
using FLRApplication.Logic;
using PagedList;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Web.UI;
using System.Drawing;

namespace FLRApplication.Logic
{
    public class filterSort
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public menu mn = new menu();

        #region PageItems
        int pageIndex = 1;
        int pageSize = 8;
        #endregion

        public IPagedList<MenuItem> AllItemsPaged(int? page)
        {
            return mn.AllItems().ToPagedList(page ?? pageIndex, pageSize);
        }

        public IPagedList<MenuItem> ContainsName(int? page, string contains)
        {
            var items = db.MenuItems.Where(x => x.ItemName
                          .Contains(contains)).ToList()
                          .ToPagedList(page ?? pageIndex, pageSize);

            return items;
        }

        public IPagedList<MenuItem> ContainsCategory(int? page, string contains)
        {
            int.TryParse(contains, out int id);

            var cateItems = db.MenuItems
                              .Where(x => x.CategID == id).ToList()
                              .ToPagedList(page ?? pageIndex, pageSize);

            return cateItems;
        }


        public IPagedList<MenuItem> AccendingPrice(int? page)
        {
            var accending = db.MenuItems
                              .OrderBy(x => x.Price).ToList()
                              .ToPagedList(page ?? pageIndex, pageSize);

            return accending;
        }

        public IPagedList<MenuItem> DescendingPrice(int? page)
        {
            var descending = db.MenuItems
                              .OrderByDescending(x => x.Price).ToList()
                              .ToPagedList(page ?? pageIndex, pageSize);

            return descending;
        }

        public IPagedList<MenuItem> CategoryAccPrice(int? page, string contains)
        {
            int.TryParse(contains, out int id);

            var accending = db.MenuItems.OrderBy(x => x.Price)
                              .Where(x => x.CategID == id).ToList()
                              .ToPagedList(page ?? pageIndex, pageSize);

            return accending;
        }

        public IPagedList<MenuItem> CategoryDescPrice(int? page, string contains)
        {
            int.TryParse(contains, out int id);

            var descending = db.MenuItems.OrderByDescending(x => x.Price)
                              .Where(x => x.CategID == id).ToList()
                              .ToPagedList(page ?? pageIndex, pageSize);

            return descending;
        }

        #region Helpers
        public class constantTexts
        {
            public const string NoItems = "Sorry, there are currently no menu items to display.";
            public const string NullItems = "Sorry, there are no menu items related to this search name.";
            public const string NullCateItems = "Sorry, there are no menu items of this category";
        }
        #endregion
    }
}