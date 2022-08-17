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
           
            using (var httpClient = new HttpClient())
            {
                //httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer),Encoding.UTF8,"application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44383/api/Account/Login", content))
                {
                    string token = await response.Content.ReadAsStringAsync();
                    if(token == "Invalid Credentials")
                    {
                        ViewBag.Message = "Incorrect UserId or Password";
                        return RedirectToAction("Index", "Home");
                    }
                    HttpContext.Session.SetString("JWToken", token);
                }
                return RedirectToAction("Get","Grocery");
            }
        }
    }
}
