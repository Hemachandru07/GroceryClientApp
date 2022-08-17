using GroceryClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GroceryClientApp.Controllers
{
    public class GroceryController : Controller
    {
        string Baseurl = "https://localhost:44383/";
        public async Task<IActionResult> Get()
        {
            List<Grocery> grocery = new List<Grocery>();
            using (var client = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");
                
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);
                HttpResponseMessage Res = await client.GetAsync("api/Grocery");

                if (Res.IsSuccessStatusCode)
                {

                    var Response = Res.Content.ReadAsStringAsync().Result;

                    grocery = JsonConvert.DeserializeObject<List<Grocery>>(Response);
                }

            }
            return View(grocery);
        }
    }
}
