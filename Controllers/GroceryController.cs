using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroceryClientApp.Data;
using GroceryClientApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GroceryClientApp.Controllers
{
    [NoDirectAccess]
    public class GroceryController : Controller
    {
        string Baseurl = "https://localhost:44383/";
        
        // GET: Grocery
        public async Task<IActionResult> Menu()
        {
            string? token = HttpContext.Session.GetString("Token");
            List<Grocery>? GroceryInfo = new List<Grocery>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Grocery");

                if (Res.IsSuccessStatusCode)
                {
                    var GroceryResponse = Res.Content.ReadAsStringAsync().Result;
                    GroceryInfo = JsonConvert.DeserializeObject<List<Grocery>>(GroceryResponse);

                }
                return View(GroceryInfo);
            }
        }

        public async Task<IActionResult> GetGroceryById(int id)
        {
            HttpContext.Session.SetInt32("GroceryID", id);
            Grocery? grocery = new Grocery();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Grocery/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    grocery = JsonConvert.DeserializeObject<Grocery>(apiResponse);
                }
            }
            return View(grocery);
        }

        public IActionResult AddGrocery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGrocery(Grocery g)
        {
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(Baseurl);
                Client.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(g), Encoding.UTF8, "application/json");
                var response = await Client.PostAsync("api/Grocery", content);

            }
            return RedirectToAction("Menu");
        }
       
        public async Task<IActionResult> EditGrocery(int id)
        {
            Grocery? grocery = new Grocery();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Grocery/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    grocery = JsonConvert.DeserializeObject<Grocery>(apiResponse);
                }
            }
            return View(grocery);
        }

        [HttpPost]
        public async Task<IActionResult> EditGrocery(Grocery g)
        {
            Grocery? grocery = new Grocery();

            using (var httpClient = new HttpClient())
            {
                int id = g.GroceryID;
                httpClient.BaseAddress = new Uri(Baseurl);
                httpClient.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(g), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("api/Grocery", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    grocery = JsonConvert.DeserializeObject<Grocery>(apiResponse);
                }
            }
            return RedirectToAction("Menu");
        }

        public async Task<IActionResult> DeleteGrocery(int id)
        {
            Grocery? grocery = new Grocery();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Grocery/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    grocery = JsonConvert.DeserializeObject<Grocery>(apiResponse);
                }
            }
            return View(grocery);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGrocery(Grocery g)
        {
            int id = g.GroceryID;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                var response = await httpClient.DeleteAsync("api/Grocery?id=" + id);
            }
            return RedirectToAction("Menu");
        }
    }
}
