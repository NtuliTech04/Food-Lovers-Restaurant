using FLRApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FLRApplication.ViewModels
{
    public class OrderViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int OrderId { get; set; }
        public IEnumerable<OrderItem> orderItems { get; set; }
        public CheckoutInfo checkoutInfo { get; set; }
        public CustomerAddress customerAddress { get; set; }
        public Payment payment { get; set; }
        public Order order { get; set; }
        public CustomerProfile customer { get; set; }
    }
}