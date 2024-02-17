using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using FLRApplication.Models;

namespace FLRApplication.Logic
{
    public class cart
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string ShoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        public string GetCartID()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp_cart_ID = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = temp_cart_ID.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }


        public void AddToCart(int Id)
        {
            ShoppingCartID = GetCartID();

            var item = db.MenuItems.Find(Id);

            if (item != null)
            {
                var cartitem = db.CartItems
                    .Where(x => x.CartID == ShoppingCartID &&
                                x.ItemID == item.ItemID)
                                .FirstOrDefault();


                if (cartitem == null)
                {
                    var cart = db.Carts.Find(ShoppingCartID);

                    if (cart == null)
                    {
                        db.Carts.Add(entity: new Cart()
                        {
                            CartId = ShoppingCartID,
                            DateCreated = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.CartItems.Add(entity: new CartItem()
                    {
                        CartItemID = Guid.NewGuid().ToString(),
                        CartID = ShoppingCartID,
                        ItemID = item.ItemID,
                        Date = DateTime.Now,
                        Quantity = 1,
                        Price = item.Price
                    });
                }
                else
                {
                    cartitem.Quantity++;
                }
                db.SaveChanges();
            }
        }

        public void incQty(string Id)
        {
            var item = db.CartItems.Find(Id);

            int qty = item.Quantity;
            item.Quantity = qty + 1;

            db.SaveChanges();
        }

        public void decQty(string Id)
        {
            var item = db.CartItems.Find(Id);

            int qty = item.Quantity;
            if (qty > 1)
            {
                item.Quantity = qty - 1;
                db.SaveChanges();
            }
        }


        public void RemoveFromCart(string Id)
        {
            ShoppingCartID = GetCartID();

            var item = db.CartItems.Find(Id);
            if (item != null)
            {
                var cartItem = db.CartItems.FirstOrDefault(predicate: x => 
                                x.CartID == ShoppingCartID && 
                                x.CartItemID == item.CartItemID);

                if (cartItem != null)
                {
                    db.CartItems.Remove(entity: cartItem);
                }
                db.SaveChanges();
            }
        }

        public void EmptyCart()
        {
            ShoppingCartID = GetCartID();

            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.CartID == ShoppingCartID))
            {
                db.CartItems.Remove(item);
            }
            try
            {
                db.Carts.Remove(db.Carts.Find(ShoppingCartID));
                db.SaveChanges();
            }
            catch (Exception) { }
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartID = GetCartID();
            return db.CartItems.ToList().FindAll(match: x => x.CartID == ShoppingCartID);
        }

        public decimal OrderTotal(string Id)
        {
            decimal amount = 0;
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.CartID == Id))
            {
                amount += (item.Price * item.Quantity);
            }
            return amount;
        }

        public string CartTotal(string Id)
        {
            decimal amount = 0;
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.CartID == Id))
            {
                amount += (item.Price * item.Quantity);
            }
            return string.Format(new CultureInfo("en-ZA", false), "{0:C}", amount); 
        }

        public string CustomTotal(string Id, decimal customPrice)
        {
            decimal amount = 0;
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.CartID == Id))
            {
                amount += (item.Price * item.Quantity);
            }
            return string.Format(new CultureInfo("en-ZA", false), "{0:C}", amount + customPrice);
        }
    }
        
}