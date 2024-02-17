using FLRApplication.Logic;
using FLRApplication.Models;
using FLRApplication.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FLRApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class PaymentsController : Controller
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


        public finance fn = new finance();

        public ActionResult MakePayment(int id)
        {
            var order = db.Orders.Find(id);
            var payment = db.Payments.Where(x => x.OrderID == id).FirstOrDefault();
            var ship = db.CheckoutInfoes.Where(x => x.CheckoutId == order.CheckoutId).FirstOrDefault();

            string tot = payment.DueAmount.ToString();

            return Redirect(PaymentLink(tot, "Order Payment | Order No: " + order.OrderID, order.OrderID));
        }

        public string PaymentLink(string totalCost, string paymentSubjetc, int order_id)
        {
            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl;
            site = "https://sandbox.payfast.co.za/eng/process?";
            merchantId = "10022900";
            merchantKey = "qq34viiias2on";
            returnUrl = "https://localhost:44326/Payments/PaymentSuccessful/" + order_id;

            //else if (paymentMode == "live")
            //{
            //    site = "https://www.payfast.co.za/eng/process";
            //    merchantId = ConfigurationManager.AppSettings["PF_MerchantID"];
            //    merchantKey = ConfigurationManager.AppSettings["PF_MerchantKey"];
            //}
            //else
            //{
            //    throw new InvalidOperationException("Payment method unknown.");
            //}
            var stringBuilder = new StringBuilder();
            //string url = Url.Action("Quotes", "Order",
            //    new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
            //    "http", Request.Url.Host);
            stringBuilder.Append("merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url= " + HttpUtility.HtmlEncode(returnUrl));
            //stringBuilder.Append("cancel_url" + HttpUtility.HtmlEncode("https://localhost:44326/Payments/PaymentSuccessful/" + order_id));
            //stringBuilder.Append("notify_url" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_NotifyURL"]));

            string amt = totalCost;
            amt = amt.Replace(",", ".");
            stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode(amt));
            stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
            stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
            stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));

            return (site + stringBuilder);
        }

        public async Task<ActionResult> PaymentSuccessful(int id)
        {
            var user = db.Orders.Find(id);
            var sender = await UserManager.FindByNameAsync(user.Email);

            OrderViewModel update = new OrderViewModel();
            fn.PaymentSuccess(id, update.order, update.payment);

            #region Email Content
            string Subject = "Payment Recieved For Order: " + user.OrderID;
            string Msg = "Hello " + sender.FirstName + "<br/><br/>We have recieved your payment. " + "Your order is being processed and will be ready in a few minutes.<br/>We have rewarded you with loyalty points equivalent to you order items total amount.<br/>Use them on you next order to get discounts.<br/>Thank you for being a loyal customer.";
            #endregion
           
            await UserManager.SendEmailAsync(sender.Id, Subject, Msg);

            return View(update);
        }

    }
}