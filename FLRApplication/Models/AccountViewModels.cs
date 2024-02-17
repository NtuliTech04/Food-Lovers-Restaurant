using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FLRApplication.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "The provided Email Address is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "The provided Email Address is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "The provided Email Address is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        [DisplayName("First Name")]
        [MinLength(3, ErrorMessage = "First Name cannot be less than 3 characters")]
        [StringLength(30, ErrorMessage = "First Name cannot be more than 30 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last Name")]
        [MinLength(3, ErrorMessage = "Last Name cannot be less than 3 characters")]
        [StringLength(30, ErrorMessage = "Last Name cannot be more than 30 characters")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("NickName")]
        [MinLength(3, ErrorMessage = "NickName cannot be less than 3 characters")]
        [StringLength(30, ErrorMessage = "NickName Name cannot be more than 30 characters")]
        public string NickName { get; set; }

        [DisplayName("RSA ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a 13-digit Identity Number")]
        [RegularExpression(@"(?<Year>[0-9][0-9])(?<Month>([0][1-9])|([1][0-2]))(?<Day>([0-2][0-9])|([3][0-1]))(?<Gender>[0-9])(?<Series>[0-9]{3})(?<Citizenship>[0-9])(?<Uniform>[0-9])(?<Control>[0-9])", ErrorMessage = "This does not appear to be an authentic Identity Number")]
        public string RSAID { get; set; }

        [Required(ErrorMessage = "Please enter a valid 10-digit Contact Number")]
        [DisplayName("Contact Number")]
        [MinLength(10, ErrorMessage = "Invalid Contact Number")]
        [StringLength(10, ErrorMessage = "Invalid Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "The provided Email Address is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please create your own secret Password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "The provided Email Address is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please create your own secret Password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "The provided Email Address is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
