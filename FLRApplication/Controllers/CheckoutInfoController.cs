using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FLRApplication.Logic;
using FLRApplication.Models;

namespace FLRApplication.Controllers
{
    public class CheckoutInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public checkout ck = new checkout();

        public ActionResult Checkout(int? id, CheckoutInfo checkout)
        {
            bool Saved = ck.Checkout(id, checkout);

            if (Saved == true)
            {
                return RedirectToAction("PlaceOrder", "Orders", new { id = checkout.CheckoutId });
            }
            else
            {
                return RedirectToAction("ViewCart", "Home");
            }
        }
    }
}