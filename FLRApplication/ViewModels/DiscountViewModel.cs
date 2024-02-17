using FLRApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FLRApplication.ViewModels
{
    public class DiscountViewModel
    {
        public Payment Payment { get; set; }
        public Point Points { get; set; }
        public Discount Discount { get; set; }
    }
}