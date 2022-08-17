using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GroceryClientApp.Models
{
    public class Receipt
    {
        [Key]
        public int ReceiptID { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer? customer { get; set; }
        public int PaymentID { get; set; }
        [ForeignKey("PaymentID")]
        public virtual Payment? payment { get; set; }
    }
}
