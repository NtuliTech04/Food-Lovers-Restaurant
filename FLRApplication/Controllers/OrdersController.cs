using FLRApplication.Logic;
using FLRApplication.Models;
using FLRApplication.ViewModels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Reflection;

namespace FLRApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public cart ct = new cart();
        public order od = new order();
        public orderView ov = new orderView();

        public async Task<ActionResult> PlaceOrder(int id)
        {
            var user = User.Identity.Name;
            var sender = await UserManager.FindByNameAsync(user);
            var newOrder = new Order();

            bool orderSave = od.NewOrder(id, user, newOrder);
            bool prePaid = od.PrePayment(newOrder.OrderID);

            #region Email Content
            string Subject = "New Order Placed: " + newOrder.OrderID;
            string Msg = "Hi " + sender.FirstName + "<br/><br/>We have recieved your order. " + "We will begin processing and prepairing once the order has been paid for.<br/>Please kindly make payment as soon as you can.<br/>Thanks for your support!";
            #endregion

            if (orderSave == true && prePaid == true)
            {
                await UserManager.SendEmailAsync(sender.Id, Subject , Msg);
                return RedirectToAction("OrderItems", new { id = newOrder.OrderID });
            }
            else
            {
                ViewBag.OrderFailed = OrderError;
                return View("ViewCart", "Home");
            }
        }
    


    public ActionResult OrderItems(int id)
        {
            bool added = od.AddOrderItem(id);

            if (added == true)
            {
                #region clean up all temporal data
                ct.EmptyCart();
                Session.Abandon();
                TempData.Clear();
                #endregion
                return RedirectToAction("OrderSummary", new { id });
            }
            else
            {
                ViewBag.OrderFailed = OrderError;
                return View("ViewCart", "Home");
            }
        }

        public ActionResult OrderSummary(int id, OrderViewModel summary)
        {
            #region Temp ViewBags
            ViewBag.PointsInsufficient = TempData["PointsInsufficient"];
            ViewBag.DiscountEligible = TempData["DiscountEligible"];
            ViewBag.LoyaltyPoints = TempData["LoyaltyPoints"];
            ViewBag.DiscountApplied = TempData["DiscountApplied"];
            ViewBag.DiscountFail = TempData["DiscountFail"];
            #endregion

            var order = ov.GetOrder(id);
            var checkout = ov.CheckoutInfo(order.CheckoutId);

            summary.OrderId = id;
            summary.payment = ov.OrderPayment(id);
            summary.orderItems = ov.GetItems(id);
            summary.checkoutInfo = ov.GetCheckoutInfo(checkout.CheckoutId);
            summary.customerAddress = ov.DeliveryAddress(checkout.ID);

            return View(summary);
        }


        public ActionResult CheckPoints(int id)
        {
            var user = HttpContext.User.Identity.Name;

            Point points = db.Points.Find(user);

            if (points == null || points.Available < 50)
            {
                TempData["PointsInsufficient"] = PointsInsufficient;
                return RedirectToAction("OrderSummary", "Orders", new { id });
            }
            TempData["DiscountEligible"] = true;
            TempData["LoyaltyPoints"] = points.Available;
            return RedirectToAction("OrderSummary", "Orders", new { id });
        }

        [HttpGet]
        public ActionResult GetDiscount(int id)
        {
            DiscountViewModel model = new DiscountViewModel();
            model.Payment = db.Payments.Where(x => x.OrderID == id).FirstOrDefault();
            model.Points = db.Points.Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            model.Discount = new Discount() { OrderID = id };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDiscount(DiscountViewModel model)
        {
            bool Disc = od.ApplyDiscount(model.Discount, model.Points);

            bool Pay = od.UpdatePayment(model.Discount,model.Payment);

            bool Point = od.UpdatePoints(model.Discount, model.Points);
        
            if (Disc == true && Pay == true && Point)
            {
                TempData["DiscountApplied"] = DiscountApplied;
                return RedirectToAction("OrderSummary", "Orders", new { id = model.Discount.OrderID });
            }
            else
            {
                TempData["DiscountFail"] = DiscountFail;
                return RedirectToAction("OrderSummary", "Orders", new { id = model.Discount.OrderID });
            }
        }

        #region Constants
        public const string OrderError = "Failed to place order, please try again...";
        public const string PointsInsufficient = "Sorry, you do not qualify for a discount at this moment.";
        public const string DiscountApplied = "Congratulations, a discount has been applied to your order.";
        public const string DiscountFail = "Discount was not applied. Discount amount was R0,00. Try again...";
        #endregion
    }
}