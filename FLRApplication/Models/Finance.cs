using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace FLRApplication.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; } //Electronic Payment
        public string PaymentStatus { get; set; } //Awaiting Payment
        public decimal OrderTotal { get; set; }
        public decimal DeliveryAmt { get; set; }
        public bool DiscountApplied { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }


        [NotMapped]
        public string TotalZar
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", OrderTotal); }
        }

        [NotMapped]
        public string DeliveryZar
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", DeliveryAmt); }
        }

        [NotMapped]
        public string DiscZar
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", DiscountAmount); }
        }
        

        [NotMapped]
        public string DueZar
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", DueAmount); }
        }

        [NotMapped]
        public string PaidZar
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", PaidAmount); }
        }



        //Class Relationships

        [ForeignKey("Customer")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public virtual CustomerProfile Customer { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }


    }

    public class Point
    {
        [Key]
        public string Email { get; set; }
        public int Available { get; set; }
        public int Used { get; set; }
        public int Total { get; set; }
    }

    public class Discount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountId { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"(((\d{1,3})(,\d{3})*)|(\d+))(.\d{2})", 
        ErrorMessage = "Incorrect Currency Format. \"Expected: 0.00\"")]
        public decimal DiscountAmount { get; set; }

        //Class Relationships

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("LoyaltPoints")]
        public string Email { get; set; }
        public virtual Point LoyaltPoints { get; set; }
    }
}