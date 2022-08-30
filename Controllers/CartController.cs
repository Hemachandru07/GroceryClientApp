using GroceryClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GroceryClientApp.Controllers
{
    [NoDirectAccess]
    public class CartController : Controller
    {
        string Baseurl = "https://localhost:44383/";
        public async Task<List<Cart>> myCart(int? id)
        {
            string? token = HttpContext.Session.GetString("Token");
            List<Cart> cart = new List<Cart>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Order/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var apiresponse = Res.Content.ReadAsStringAsync().Result;
                    cart = JsonConvert.DeserializeObject<List<Cart>>(apiresponse);
                }
            }
            return cart;
        }
        public async Task<IActionResult> GetMyCart()
        {
           
            var id = HttpContext.Session.GetInt32("CustomerID");
            List<Cart>? cart = await myCart(id);
            return View(cart);
        }

        public IActionResult AddToCart()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Cart cart)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                ViewBag.CustomerID = HttpContext.Session.GetInt32("CustomerID");
                cart.CustomerID = @ViewBag.CustomerID;
                TempData["CustomerId"] = cart.CustomerID;
                ViewBag.GroceryID = HttpContext.Session.GetInt32("GroceryID");
                cart.GroceryID = @ViewBag.GroceryID;
                StringContent content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Order/AddToCart", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetMyCart", "Cart");
                }
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteCart(int id)
        {
            Cart? cart = new Cart();
            TempData["Id"] = id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                using (var response = await client.GetAsync("api/Order/GetCartById?id=" + id))
                {
                    var apiresponse = response.Content.ReadAsStringAsync().Result;
                    cart = JsonConvert.DeserializeObject<Cart>(apiresponse);
                }
            }
            return View(cart);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteCart(Cart c)
        {
            int id = Convert.ToInt32(TempData["Id"]);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                await client.DeleteAsync("api/Order/" + id);
                return RedirectToAction("GetMyCart");
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditCart(int id)
        {
            Cart? cart = new Cart();
            TempData["Id"] = id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                using (var response = await client.GetAsync("api/Order/GetCartById?id=" + id))
                {
                    var apiresponse = response.Content.ReadAsStringAsync().Result;
                    cart = JsonConvert.DeserializeObject<Cart>(apiresponse);
                }
            }
            return View(cart);
        }

        [HttpPost]
        public async Task<ActionResult> EditCart(Cart cart)
        {
            int id = Convert.ToInt32(TempData["Id"]);
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(Baseurl);
                Client.DefaultRequestHeaders.Clear();
                //int id = (int)cart.CartID;
                StringContent content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
                var response = await Client.PutAsync("api/Order/" + id, content);
                
            }
            return RedirectToAction("GetMyCart");
        }

        

    }
}
