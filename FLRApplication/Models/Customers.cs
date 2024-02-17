using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FLRApplication.Models
{
    public class CustomerProfile
    {
        [Key]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("NickName")]
        public string NickName { get; set; }

        [DisplayName("RSA ID")]
        public string RSAID { get; set; }

        [Display(Name = "Date Of Birth")]
        public string BirthDate { get; set; }

        [Display(Name = "Age")]
        public string Age { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [DisplayName("Profile Photo")]
        public byte[] ProfilePhoto { get; set; }

        public ICollection<CustomerAddress> Addresses { get; set; }
    }

    public class CustomerAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayName("Street/House")]
        [Required(ErrorMessage = "Enter your street/house number.")]
        [MaxLength(10)]
        public string StreetNo { get; set; }

        [DisplayName("Suburb")]
        [Required(ErrorMessage = "What is your surrounding area called?")]
        public string Suburb { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Which City is this area located?")]
        public string City { get; set; }

        [DisplayName("Province")]
        [Required(ErrorMessage = "Which Province is your City located.")]
        public string Province { get; set; }

        [DisplayName("Postal Code")]
        [MinLength(4, ErrorMessage = "Invalid Postal Code")]
        [StringLength(4, ErrorMessage = "Invalid Postal Code")]
        [Required(ErrorMessage = "Enter a 4 digit postal/area code")]
        public string PosctalCode { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "You forgot to tell us which country are you from.")]
        public string Country { get; set; }


        //Class Relationships

        [DisplayName("Email")]
        public string Email { get; set; }
        public virtual CustomerProfile Profile { get; set; }
    }

}