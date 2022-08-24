using GroceryClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GroceryClientApp.Controllers
{
    public class BillController : Controller
    {
        string Baseurl = "https://localhost:44383/";
        CartController c=new CartController();
        public async Task<ActionResult> Payment()
        {
            var id = HttpContext.Session.GetInt32("CustomerID");
            List<Cart> carts = await c.myCart(id);
            float? pay =0;
            foreach (Cart cart in carts)
            {
                pay+= cart.UnitPrice;
            }
            Payment p=new Payment();
            p.TotalAmount = pay;
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(Payment payment)
        {
            Payment p = new Payment();
            p.CustomerID = (int)HttpContext.Session.GetInt32("CustomerID");
            p.CardNumber = payment.CardNumber;

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(Baseurl);
                Client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                var response = await Client.PostAsync("api/Bill/Payment", content);

            }
            return RedirectToAction("Menu","Grocery");
        }

        [HttpPost]
        public async Task<IActionResult> Receipt()
        {
            var id = HttpContext.Session.GetInt32("CustomerID");
            List<Cart> cart = new List<Cart>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/Order/" + id);
                if (res.IsSuccessStatusCode)
                {
                    var apiresponse = res.Content.ReadAsStringAsync().Result;
                    cart = JsonConvert.DeserializeObject<List<Cart>>(apiresponse);
                }
            }
            List<Payment> payment = new List<Payment>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/Order/" + id);
                if (res.IsSuccessStatusCode)
                {
                    var apiresponse = res.Content.ReadAsStringAsync().Result;
                    cart = JsonConvert.DeserializeObject<List<Cart>>(apiresponse);
                }
            }
        }
    }
}
