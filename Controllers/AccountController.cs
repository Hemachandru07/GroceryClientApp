using GroceryClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Routing;
using NuGet.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Mail;
using System.Net;

namespace GroceryClientApp.Controllers
{
    //---------------------------------------
    //No direct access
    //public class NoDirectAccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var canAcess = false;

    //        // check the refer
    //        var referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
    //        if (!string.IsNullOrEmpty(referer))
    //        {
    //            var rUri = new System.UriBuilder(referer).Uri;
    //            var req = filterContext.HttpContext.Request;
    //            if (req.Host.Host == rUri.Host && req.Host.Port == rUri.Port && req.Scheme == rUri.Scheme)
    //            {
    //                canAcess = true;
    //            }
    //        }

    //        // ... check other requirements

    //        if (!canAcess)
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
    //        }
    //    }
    //}
    //-----------------------------------------
    
    public class AccountController : Controller
    {
       
        private readonly ISession Session;
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            Session = httpContextAccessor.HttpContext.Session;
        }
        string Baseurl = "https://localhost:44383/";
        //string Baseurl = "https://app-chandruapi.azurewebsites.net/";
        public IActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(Customer? customer)
        {
            JWTToken jwt = new JWTToken();
            customer.CPassword = customer.Password;
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
        //[NoDirectAccess]
        public IActionResult RegisterUser()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> RegisterUser(Customer customer)
        {
            //--------------
            
            var senderEmail = new MailAddress("grocerymarket12@gmail.com", "Grocery Admin");
            var receiverEmail = new MailAddress(customer.CustomerEmail, "Receiver");
            var password = "dqmizaerwutbfpmq";
            //String b = "https://localhost:44369/Account/LoginUser";
            String b = "https://app-chandruapi.azurewebsites.net/Account/LoginUser";

            var sub = "Hello " + customer.CustomerName + "! Welcome to Grocery Market";
            var body = "Your User Id: " + customer.CustomerEmail + " And your password is :" + customer.Password + " Login link " + b;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
                ViewBag.Message = String.Format("Registered Successfully!!\\ Please Check Your Mail to login.");
            }
            //----------------------
            customer.CPassword = customer.Password;
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
        //[NoDirectAccess]
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
                    HttpContext.Session.SetString("Admin", admin.EmailID);
                    return RedirectToAction("Menu", "Grocery");

                }
                ViewBag.ErrorMessage = "Invalid Credentials";
                return View();
                
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
