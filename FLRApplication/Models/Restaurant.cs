using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FLRApplication.Models
{
    public class Category
    {
        [Key]
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategID { get; set; }

        [DisplayName("Category")]
        public string CategName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }



        //Class Relationships
        public ICollection<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        [Key]
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [DisplayName("Name")]
        [StringLength(20, ErrorMessage = "Item name cannot be more than 15 characters, try making it simple.")]
        public string ItemName { get; set; }

        [DisplayName("Description")]
        public string ItemDesc { get; set; }

        [DisplayName("Image")]
        public byte[] ItemImage { get; set; }

        [DisplayName("Price")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"(((\d{1,3})(,\d{3})*)|(\d+))(.\d{2})",
        ErrorMessage = "Incorrect Currency Format. \"Try: 0.00\"")]
        public decimal Price { get; set; }

        public bool Customizable { get; set; }

        public bool OnSpecial { get; set; }

        [MinLength(50, ErrorMessage = "Promo description cannot be less than 50 characters")]
        [StringLength(60, ErrorMessage = "Promo description cannot be more than 60 characters")]
        public string PromoDesc { get; set; }   

        [NotMapped]
        public string PromoStatus { get; set; }

        [NotMapped]
        public string ItemPrice
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", Price); }
        }


        
        //Class Relationships

        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int CategID { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }

    public class Ingredient
    {
        [Key]
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IngredID { get; set; }

        [DisplayName("Name")]
        public string IngredName { get; set; }

        [DisplayName("Description")]
        public string IngreDesc { get; set; }

        [DisplayName("Sold In")]
        public string SoldIn { get; set; } //Mass/Quantity

        [DisplayName("Initial Amount")]
        public string InitAmount { get; set; }

        [DisplayName("Price")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"(((\d{1,3})(,\d{3})*)|(\d+))(.\d{2})", 
        ErrorMessage = "Incorrect Currency Format. \"Try: 0.00\"")]
        public decimal Price { get; set; }

        [NotMapped]
        public string IngredPrice
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", Price); }
        }

        [DisplayName("Image")]
        public byte[] IngredImage { get; set; }


        //Class Relationships
        [ForeignKey("MenuItem")]
        [DisplayName("Menu Item")]
        public int ItemID { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }

    public class Cart
    {
        public string CartId { get; set; }

        public DateTime DateCreated { get; set; }


        //Class Relationships
        public ICollection<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        [Key]
        [DisplayName("Cart Item")]
        public string CartItemID { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        [DisplayName("Preferences")]
        public string Preferences { get; set; }

        public bool Customized { get; set; }

        [DisplayName("Quantity Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        [NotMapped]
        public string QuantityPrice
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", Price); }
        }


        //Class Relationship
     
        [ForeignKey("MenuItem")]
        [DisplayName("Menu Item")]
        public int ItemID { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        [ForeignKey("Cart")]
        [DisplayName("Cart")]
        public string CartID { get; set; }
        public Cart Cart { get; set; }
    }

    public class Order
    {
        [Key]
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [DisplayName("Created On")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DisplayName("Prep-Time")]
        public string PrepTime { get; set; }

        [DisplayName("Order Status")]
        public string OrderStatus { get; set; }

        [DisplayName("Receipt Confirmation")]
        public string ReceiptConfirmation{ get; set; }



        //Class Relationships
        [DisplayName("Email")]
        [ForeignKey("Customer")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public virtual CustomerProfile Customer { get; set; }

        [DisplayName("CheckoutId")]
        [ForeignKey("Checkout")]
        public int CheckoutId { get; set; }
        public CheckoutInfo Checkout { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Payment> Payments { get; set; } 
    }

    public class OrderItem
    {
        [Key]
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OItemID { get; set; }

        [DisplayName("Changes")]
        public string OrderPrefs { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        [DisplayName("Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [NotMapped]
        public string ZarPrice
        {
            get { return string.Format(new CultureInfo("en-ZA", false), "{0:C}", Price); }
        }


        //Class Relationships
        [ForeignKey("MenuItem")]
        [DisplayName("Menu Item")]
        public int ItemID { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        [ForeignKey("Order")]
        [DisplayName("Order")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
    }
}