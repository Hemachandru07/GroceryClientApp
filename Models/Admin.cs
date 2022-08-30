using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryClientApp.Models
{
    public class Admin
    {
        [Key]
        [Display(Name = "Admin ID")]
        public int AdminID { get; set; }

        [Required(ErrorMessage = "*Requires Name")]
        [Display(Name = "Admin Name")]
        public string? AdminName { get; set; }

        [Required(ErrorMessage = "*Requires Email-ID")]
        [Display(Name = "Email-ID")]
        [DataType(DataType.EmailAddress)]
        public string? EmailID { get; set; }

        [Required(ErrorMessage = "*Requires Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
          ErrorMessage = "Password must contains one Uppercase,one Lowercase and one Specialcharacter")]
        public string? Password { get; set; }
    }
}
