using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryClientApp.Models
{
    public class Admin
    {
        [Key]
        [Display(Name = "Admin ID")]
        public int AdminID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Admin Name")]
        public string? AdminName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email-ID")]
        [DataType(DataType.EmailAddress)]
        public string? EmailID { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
          ErrorMessage = "Password must contains one Uppercase,one Lowercase and one Specialcharacter")]
        public string? Password { get; set; }
    }
}
