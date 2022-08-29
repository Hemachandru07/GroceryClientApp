using GroceryClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Routing;
using NuGet.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GroceryClientApp.Controllers
{
    //---------------------------------------
    //No direct access
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var canAcess = false;

            // check the refer
            var referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                var rUri = new System.UriBuilder(referer).Uri;
                var req = filterContext.HttpContext.Request;
                if (req.Host.Host == rUri.Host && req.Host.Port == rUri.Port && req.Scheme == rUri.Scheme)
                {
                    canAcess = true;
                }
            }

            // ... check other requirements

            if (!canAcess)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
    //-----------------------------------------
    [NoDirectAccess]
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
        public async Task<IActionResult> LoginUser(Customer? customer)
        {
            JWTToken jwt = new JWTToken();
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/Account/Login", content);
                if (Res.IsSuccessStatusCode)
                {
                    string apiResponse = await Res.Content.ReadAsStringAsync();
                    jwt = JsonConvert.DeserializeObject<JWTToken>(apiResponse);
                    if(jwt == null)
                    {
                        ViewBag.ErrorMessage = "Invalid Credentials";
                        return View();
                    }
                    HttpContext.Session.SetString("Customer", jwt.customer.CustomerEmail);
                    HttpContext.Session.SetInt32("CustomerID", jwt.customer.CustomerID);
                    string Token = jwt.Token;
                    HttpContext.Session.SetString("Token", Token);
                    return RedirectToAction("Menu", "Grocery");
                }
                return View();
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
