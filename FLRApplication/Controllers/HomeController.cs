using System.Web;
using System.Web.Mvc;
using FLRApplication.Utilities;
using FLRApplication.Models;
using FLRApplication.Logic;
using System.Collections.Generic;
using System.Linq;
using System;
using PagedList;
using System.Data.Entity;
using PagedList.Mvc;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace FLRApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public menu mn = new menu();
        public ingredient ig = new ingredient();
        public filterSort fs = new filterSort();
        public cart ct = new cart();

        public string ShoppingCartID { get; set; }


        [HttpGet]
        public ActionResult Index(int? page, string searchCate, string seachPrice, string searchName)
        {
            ViewBag.Categories = new SelectList(db.Categories, "CategID", "CategName");
            ViewBag.SpecialCount = mn.OnPromotion().Count();

            var itemList = fs.AllItemsPaged(page);

            if (itemList.Count > 0)
            {
                if (!searchName.IsEmpty())
                {
                    ModelState.Clear();

                    var srchItems = fs.ContainsName(page, searchName.Trim());

                    if (srchItems.Count > 0)
                    {
                        return View(srchItems);
                    }
                    else
                    {
                        ViewBag.NullItems = filterSort.constantTexts.NullItems;
                        return View(srchItems);
                    }
                }


                if (!searchCate.IsEmpty() && seachPrice.IsNullOrWhiteSpace())
                {
                    var categoryItems = fs.ContainsCategory(page, searchCate);

                    if (categoryItems.Count > 0)
                    {
                        return View(categoryItems);
                    }
                    else
                    {
                        ViewBag.NullCateItems = filterSort.constantTexts.NullCateItems;
                        return View(categoryItems);
                    }
                }


                if (!seachPrice.IsEmpty())
                {
                    if (!searchCate.IsEmpty())
                    {
                        if (seachPrice == "descending")
                        {
                            return View(fs.CategoryDescPrice(page, searchCate));
                        }
                        else
                        {
                            return View(fs.CategoryAccPrice(page, searchCate));
                        }
                    }
                    else
                    {
                        if (seachPrice == "descending")
                        {
                            return View(fs.DescendingPrice(page));
                        }
                        else
                        {
                            return View(fs.AccendingPrice(page));
                        }
                    }
                }
                return View(itemList);
            }
            else
            {
                ViewBag.NoItems = filterSort.constantTexts.NoItems;
                return View(itemList);
            }
        }

        public ActionResult AddToCart(int Id)
        {
            var item = db.MenuItems.Find(Id);

            if (item != null)
            {
                ct.AddToCart(Id);
                return RedirectToAction("ViewCart");
            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }

        public ActionResult ViewCart(List<CartItem> cartItems)
        {
            ShoppingCartID = ct.GetCartID();
            Session["CartTotal"] = ct.CartTotal(ShoppingCartID);
            Session["MenuItemCount"] = ct.GetCartItems().FindAll(x => x.CartID == ShoppingCartID).Sum(x => x.Quantity);
            cartItems = db.CartItems.Include(x => x.MenuItem).ToList().FindAll(x => x.CartID == ShoppingCartID);

            return View(cartItems);
        }

        public ActionResult IncQty(string Id)
        {
            ct.incQty(Id);
            return RedirectToAction("ViewCart");
        }

        public ActionResult DecQty(string Id)
        {
            ct.decQty(Id);
            return RedirectToAction("ViewCart");
        }

        public ActionResult CustomizeItem(int Id, string cartItem)
        {
            if (TempData.Count() == 0)
            {
                TempData["MenuItem"] = ig.Menuitem(Id);
                TempData["Ingredients"] = ig.GetIngredients(Id);
                TempData["Prefs"] = new Dictionary<int, string>();
                TempData["Price"] = new Dictionary<int, decimal>();

                Session["cartItem"] = cartItem;
            }
            TempData.Keep();

            ShoppingCartID = ct.GetCartID();
            var customPrice = TempData["Price"] as Dictionary<int, decimal>;

            Session["CartTotal"] = ct.CustomTotal(ShoppingCartID, customPrice.Values.Sum());

            return View();
        }

        public ActionResult DontCustomize()
        {
            TempData.Clear();
            return RedirectToAction("ViewCart", "Home");
        }

        public ActionResult RestoreDefaults(int id)
        {
            TempData.Clear();
            string cartItem = ct.GetCartID();
            return RedirectToAction("CustomizeItem", "Home", new { id, cartItem });
        }

        public ActionResult SubmitCustoms()
        {
            var cartItem = db.CartItems.Find(Convert.ToString(Session["cartItem"]));

            var prefs = TempData["Prefs"] as Dictionary<int, string>;
            var ingredPrices = TempData["Price"] as Dictionary<int, decimal>;

            var prefList = new List<string>();

            foreach (KeyValuePair<int, string> pref in prefs)
            {
                if(pref.Value != null)
                {
                    prefList.Add(pref.Value);
                }
            }

            if (prefList.Count != 0) 
            {
                cartItem.Preferences = string.Join(", ", prefList.ToArray());
                cartItem.Price = cartItem.Price + ingredPrices.Values.Sum();
                cartItem.Customized = true;

                db.SaveChanges();
            }
            TempData.Clear();

            return RedirectToAction("ViewCart");
        }

        public ActionResult PlusIngred(int Id)
        {
            var tempData = TempData["Ingredients"] as List<Ingredient>;
            var Prefs = TempData["Prefs"] as Dictionary<int, string>;
            var PriceDict = TempData["Price"] as Dictionary<int, decimal>;

            var ingred = tempData.Find(x => x.IngredID == Id);

            TempData["priceEach"] = ingred.Price;
            decimal priceEach = Convert.ToDecimal(TempData["priceEach"]);

            if (ingred.InitAmount.IsInt())
            {
                int qty = int.Parse(ingred.InitAmount);

                if (ingred.InitAmount == "0")
                {
                    ingred.InitAmount = (qty + 1).ToString();
                    Prefs[ingred.IngredID] = null;
                    PriceDict[ingred.IngredID] = 0;
                }
                else
                {
                    ingred.InitAmount = (qty + 1).ToString();
                    Prefs[ingred.IngredID] = qty + " Extra " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = qty * priceEach;
                }
            }
            else
            {
                string[] amt = { "None", "Light", "Regular", "Extra" };

                for (int i = 0; i < amt.Length; i++)
                {
                    if (ingred.InitAmount == amt[i])
                    {
                        ingred.InitAmount = amt[i + 1];
                        break;
                    }
                }

                if (ingred.InitAmount == "Light")
                {
                    Prefs[ingred.IngredID] = "Light " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = (priceEach / 2) - priceEach;
                }
                else if (ingred.InitAmount == "Regular")
                {
                    Prefs[ingred.IngredID] = null;
                    PriceDict[ingred.IngredID] = 0;
                }
                else if (ingred.InitAmount == "Extra")
                {
                    Prefs[ingred.IngredID] = "Extra " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = priceEach;
                }
            }

            return RedirectToAction("CustomizeItem", new { Id = ingred.MenuItem.ItemID });
        }


        public ActionResult MinusIngred(int Id)
        {
            var tempData = TempData["Ingredients"] as List<Ingredient>;
            var Prefs = TempData["Prefs"] as Dictionary<int, string>;
            var PriceDict = TempData["Price"] as Dictionary<int, decimal>;

            var ingred = tempData.Find(x => x.IngredID == Id);

            TempData["priceEach"] = ingred.Price;
            decimal priceEach = Convert.ToDecimal(TempData["priceEach"]);

            if (ingred.InitAmount.IsInt())
            {
                int qty = int.Parse(ingred.InitAmount);
                ingred.InitAmount = (qty - 1).ToString();

                if (ingred.InitAmount == "0")
                {
                    Prefs[ingred.IngredID] = "No " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = -priceEach;
                }
                else if (ingred.InitAmount == "1")
                {
                    Prefs[ingred.IngredID] = null;
                    PriceDict[ingred.IngredID] = 0;
                }
                else
                {
                    Prefs[ingred.IngredID] = (int.Parse(ingred.InitAmount) - 1) + " Extra " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = (int.Parse(ingred.InitAmount) - 1) * priceEach;
                }
            }
            else
            {
                string[] amt = { "None", "Light", "Regular", "Extra" };

                for (int i = 0; i < amt.Length; i++)
                {
                    if (ingred.InitAmount == amt[i])
                    {
                        ingred.InitAmount = amt[i - 1];
                        break;
                    }
                }

                if (ingred.InitAmount == "None")
                {
                    Prefs[ingred.IngredID] = "No " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = -priceEach;
                }
                else if (ingred.InitAmount == "Light")
                {
                    Prefs[ingred.IngredID] = "Light " + ingred.IngredName;
                    PriceDict[ingred.IngredID] = (priceEach / 2) - priceEach;
                }
                else if (ingred.InitAmount == "Regular")
                {
                    Prefs[ingred.IngredID] = null;
                    PriceDict[ingred.IngredID] = 0;
                }
            }

            return RedirectToAction("CustomizeItem", new { Id = ingred.MenuItem.ItemID });
        }


        public ActionResult RemoveFromCart(string Id)
        {
            var item = db.CartItems.Find(Id);
            if (item != null)
            {
                ct.RemoveFromCart(Id: Id);
                //Session.Clear();
                //TempData.Clear();
                return RedirectToAction("ViewCart");
            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }

        public ActionResult EmptyCart()
        {
            ct.EmptyCart();
            Session.Clear();
            TempData.Clear();
            return RedirectToAction("ViewCart");
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