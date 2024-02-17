using FLRApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FLRApplication.Logic
{
    public class orderView
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public CheckoutInfo CheckoutInfo(int id)
        {
            return db.CheckoutInfoes
                     .Where(x => x.CheckoutId == id)
                     .FirstOrDefault();
        }

        public Order GetOrder(int id)
        {
            return db.Orders.Find(id);
        }

        public Payment OrderPayment(int id)
        {
            return db.Payments.ToList().Find(x => x.OrderID == id);
        }

        public IEnumerable<OrderItem> GetItems(int id)
        {
            return db.OrderItems
                     .Where(o => o.OrderID == id)
                     .ToList();
        }

        public CheckoutInfo GetCheckoutInfo(int id)
        {
            return db.CheckoutInfoes
                     .Where(x => x.CheckoutId == id)
                     .FirstOrDefault();
        }

        public CustomerAddress DeliveryAddress(int? id)
        {
            return db.Addresses
                     .Where(x => x.ID == id)
                     .FirstOrDefault();
        }
    }
}