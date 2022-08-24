using GroceryClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Routing;

namespace GroceryClientApp.Controllers
{
    public class AccountController : Controller
    {
       
        private readonly ISession Session;
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            Session = httpContextAccessor.HttpContext.Session;
        }
        string Baseurl = "https://localhost:44383/";
        public IActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(Customer customer)
        {
            Customer c = new Customer();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/Account/Login", content);
                if (Res.IsSuccessStatusCode)
                {
                    string apiResponse = await Res.Content.ReadAsStringAsync();
                    c = JsonConvert.DeserializeObject<Customer>(apiResponse);
                    HttpContext.Session.SetString("Customer", customer.CustomerEmail);
                    HttpContext.Session.SetInt32("CustomerID", c.CustomerID);
                    return RedirectToAction("Menu", "Grocery");

                }
                return RedirectToAction("LoginUser");

            }
        }
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Account/Register", content);
                //ViewBag.SuccessMessage = "Thanks for Registration";
                return RedirectToAction("LoginUser", "Account");
            }
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAdmin(Admin admin)
        {
            Admin a = new Admin();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(admin), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/Account/AdminLogin", content);
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Menu", "Grocery");

                }
                return RedirectToAction("LoginAdmin");
                
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
