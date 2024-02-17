using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FLRApplication.Models
{
    public class CheckoutInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int CheckoutId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        public string Receival { get; set; } //Delivery or Collection

        public decimal DeliveryCost { get; set; }


        //Class Relationships

        [DisplayName("AddressId")]
        public int? ID { get; set; }
        public virtual CustomerAddress CustomerAddress { get; set; }
    }
}