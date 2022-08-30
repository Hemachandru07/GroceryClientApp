using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryClientApp.Models
{
    public class Grocery
    {
        [Key]
        [Display(Name = "Grocery ID")]
        public int GroceryID { get; set; }
        [Display(Name = "Grocery Name")]
        [Required(ErrorMessage ="Enter Valid Grocery Name")]
        public string? GroceryName { get; set; }
        [Required(ErrorMessage ="Enter Valid Price")]
        public float? Price { get; set; }
        [Range(0, 500, ErrorMessage = "Stock must be between 0-500")]
        public int? Stock { get; set; }
        public List<Cart>? cart { get; set; }
        public List<Receipt>? receipt { get; set; }
    }
}
