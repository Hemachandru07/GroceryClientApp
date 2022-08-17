using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryClientApp.Models
{
    public class Cart
    {
        [Key]
        [Display(Name = "Cart ID")]
        public int CartID { get; set; }

        [Display(Name = "Cart Type ID")]
        [Required(ErrorMessage = "*")]
        public string? CartTypeId { get; set; }
        [Required(ErrorMessage = "*")]
        public int GroceryID { get; set; }
        [ForeignKey("GroceryID")]
        public virtual Grocery? grocery { get; set; }
        [Required(ErrorMessage = "*")]
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer? customer { get; set; }
        [Required(ErrorMessage = "*")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Unit Price")]
        public float? UnitPrice
        {
            get; set;
        }
    }
}
