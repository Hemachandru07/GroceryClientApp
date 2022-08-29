namespace GroceryClientApp.Models
{
    public class JWTToken
    {
        public Customer? customer { get; set; }
        public string? Token { get; set; }
    }
}
