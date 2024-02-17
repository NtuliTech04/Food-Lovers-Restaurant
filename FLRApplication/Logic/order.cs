using FLRApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FLRApplication.ViewModels;

namespace FLRApplication.Logic
{
    public class checkout
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool Checkout(int? id, CheckoutInfo checkout)
        {
            try
            {
                if (id != null)
                {
                    checkout.ID = id;
                    checkout.Receival = "Delivery";
                    checkout.DeliveryCost = 20;
                }
                else
                {
                    checkout.Receival = "Collection";
                    checkout.DeliveryCost = 0;
                }

                checkout.DateTime = DateTime.Now;

                db.CheckoutInfoes.Add(checkout);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }
    }

    public class order
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public cart ct = new cart();

        public bool NewOrder(int id, string email, Order order)
        {
            try
            {
                order.Email = email;
                order.DateCreated = DateTime.Now;
                order.CheckoutId = id;
                order.OrderStatus = "Pending";
                order.ReceiptConfirmation = "Not Confirmed";

                db.Orders.Add(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool PrePayment(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                decimal orderTotal = ct.OrderTotal(ct.GetCartID());
                decimal deliveryAmt = db.CheckoutInfoes.Find(order.CheckoutId).DeliveryCost;

                db.Payments.Add(new Payment()
                {
                    Email = order.Email,
                    OrderID = order.OrderID,
                    PaymentMethod = "Electronic Payment",
                    PaymentStatus = "Awaiting",
                    OrderTotal = orderTotal,
                    DeliveryAmt = deliveryAmt,
                    DueAmount = orderTotal + deliveryAmt,
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }

        }

        public bool AddOrderItem(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                var items = ct.GetCartItems();

                foreach (var item in items)
                {
                    db.OrderItems.Add(new OrderItem()
                    {
                        OrderID = order.OrderID,
                        ItemID = item.ItemID,
                        OrderPrefs = item.Preferences,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool ApplyDiscount(Discount discount, Point point)
        {
            try
            {
                discount.Email = point.Email;
                db.Discounts.Add(discount);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool UpdatePayment(Discount discount, Payment payment)
        {
            payment = db.Payments.Where(x => x.OrderID == discount.OrderID).FirstOrDefault();

            try
            {
                payment.DiscountAmount = discount.DiscountAmount;

                if (payment.DiscountAmount > 0)
                {
                    payment.DiscountApplied = true;
                    payment.DueAmount = payment.OrderTotal
                                      + payment.DeliveryAmt
                                      - payment.DiscountAmount;
                }
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; } 
        }

        public bool UpdatePoints(Discount discount, Point point) 
        {
            try
            {
                point = db.Points.Where(x => x.Email == discount.Email).FirstOrDefault();

                int getUsedPoints = Convert.ToInt32(discount.DiscountAmount * 10);

                point.Available -= getUsedPoints;
                point.Used += getUsedPoints;
                point.Total = point.Available + point.Used;

                db.Entry(point).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}