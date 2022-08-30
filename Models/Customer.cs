using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryClientApp.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }


        [RegularExpression(@"^[A-Z][\sA-Z]+$", ErrorMessage = "Numbers and special characters are not allowed")]
        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "*Requires Name")]
        public string? CustomerName { get; set; }


        [Display(Name = "Email-ID")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "*Requires Email-ID")]
        public string? CustomerEmail { get; set; }


        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Enter Valid Number")]

        [Required(ErrorMessage = "*Requires Mobile Number")]
        public long MobileNumber { get; set; }

        [Required(ErrorMessage = "*Address")]
        public string? Address { get; set; }


        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
           ErrorMessage = "Password must contains one Uppercase,one Lowercase and one Specialcharacter")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*Requires Password")]
        public string? Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Password Not Matching")]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "*Requires Password")]
        public string? CPassword { get; set; }


        [Display(Name = "Cart Type ID")]
        public string? CartTypeId { get; set; }
        public List<Cart>? cart { get; set; }
        public List<Payment>? Payment { get; set; }
        public List<Receipt>? Receipt { get; set; }
    }
}
