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
            Receipt r = new Receipt();
            r.CustomerID = (int)HttpContext.Session.GetInt32("CustomerID");
            p.CustomerID = (int)HttpContext.Session.GetInt32("CustomerID");
            p.CardNumber = payment.CardNumber;

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(Baseurl);
                Client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                var response = await Client.PostAsync("api/Bill/Payment", content);
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
                await Client.PostAsync("api/Bill", content1);
            }
            return RedirectToAction("Receipt");
        }
      
        public async Task<IActionResult> Receipt()
        {
            var id = HttpContext.Session.GetInt32("CustomerID");
            List<Cart> cart = new List<Cart>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
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
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/Bill/GetPayment?cid=" + id);
                if (res.IsSuccessStatusCode)
                {
                    var apiresponse = res.Content.ReadAsStringAsync().Result;
                    payment = JsonConvert.DeserializeObject<List<Payment>>(apiresponse);
                }
            }
            var p = payment.LastOrDefault();
            ViewBag.Total = p.TotalAmount;

            List<Grocery> grocery = new List<Grocery>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/Grocery");
                if (res.IsSuccessStatusCode)
                {
                    var apiresponse = res.Content.ReadAsStringAsync().Result;
                    grocery = JsonConvert.DeserializeObject<List<Grocery>>(apiresponse);
                }
            }
            
            var a = new List<object>();
            foreach(var item in cart)
            {
                foreach(var item2 in grocery)
                {
                    if(item.GroceryID == item2.GroceryID)
                        a.Add(item2.GroceryName);
                }
            }
            var b = new List<object>();
            foreach (var item in cart)
            {
                foreach (var item2 in grocery)
                {
                    if (item.GroceryID == item2.GroceryID)
                        b.Add(item2.Price);
                }
            }
            var c = new List<object>();
            foreach (var item in cart)
            {
                foreach (var item2 in grocery)
                {
                    if (item.GroceryID == item2.GroceryID)
                        c.Add(item.Quantity);
                }
            }
            ViewBag.Collection = a;
            ViewBag.CollectionPrice = b;
            ViewBag.CollectionQuantity = c;
            Customer customer = new Customer();
            customer.CustomerID = (int)id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PutAsync("api/Bill/UpdateTypeId",content);
                
            }
            return View();
        }
        public async Task<ActionResult<List<Receipt>>> GetMyOrders()
            {
            List<Receipt> receipt = new List<Receipt>();
            var id = HttpContext.Session.GetInt32("CustomerID");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                using (var response = await client.GetAsync("api/Bill/GetMyOrders?cid=" + id))
                {
                    var apiresponse = response.Content.ReadAsStringAsync().Result;
                    receipt = JsonConvert.DeserializeObject<List<Receipt>>(apiresponse);
                }
            }
            return View(receipt);
        }

    }
}
