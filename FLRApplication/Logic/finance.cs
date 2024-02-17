using FLRApplication.Models;
using FLRApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FLRApplication.Logic
{
    public class finance
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool PaymentSuccess(int id, Order order, Payment payment)
        {
            UpdateOrder(id, order);
            UpdatePayment(id, payment);
            AwardPoints(id);

            return true;
        }

        public Order UpdateOrder(int id, Order order)
        {
            order = db.Orders.Find(id);

            order.OrderStatus = "Received";

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return order;
        }

        public Payment UpdatePayment(int id, Payment payment)
        {
               payment = db.Payments
                           .Where(x => x.OrderID == id)
                           .FirstOrDefault();

            payment.PaymentStatus = "Paid";
            payment.PaidAmount = payment.DueAmount;
            payment.DueAmount = 0;

            db.Entry(payment).State = EntityState.Modified;
            db.SaveChanges();

            return payment;
        }

        public Point AwardPoints(int id)
        {
            var payment = db.Payments.Find(id);
            var point = db.Points.Find(payment.Email);

            if(point == null)
            {
                db.Points.Add(new Point()
                {
                    Email = payment.Email,
                    Available = Convert.ToInt32(payment.OrderTotal)
                });
            }
            else
            {
                point.Available += (Convert.ToInt32(payment.OrderTotal));
                point.Total = point.Available + point.Used;

                db.Entry(point).State = EntityState.Modified;
            }

            db.SaveChanges();

            return point;
        }
    }
}