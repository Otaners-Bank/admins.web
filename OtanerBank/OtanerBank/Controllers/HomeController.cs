using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OtanerBank.Models;

namespace OtanerBank.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient http = new HttpClient(); // to call the api later

        public async Task<IActionResult> Index()
        {
            try
            {
                string response = await http.GetStringAsync("https://localhost:44329/Clients");
                List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(response);

                return View(Clients.ToList());
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> Details(string CPF)
        {
            try
            {
                string response = await http.GetStringAsync("https://localhost:44329/Clients/" + CPF);
                Client client = JsonConvert.DeserializeObject<Client>(response);

                return View(client);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> Edit(string CPF)
        {
            try
            {
                string response = await http.GetStringAsync("https://localhost:44329/Clients/" + CPF);
                Client client = JsonConvert.DeserializeObject<Client>(response);

                return View(client);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> SaveUpdates(Client clientData)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(clientData); // Serializing object to put in the JsonObject
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var message = await http.PutAsync("https://localhost:44329/Clients/" + clientData.CPF, httpContent);

                return RedirectToAction("Index"); // and returns to the Home Page
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegister(Client client)
        {
            try
            {
                return View("");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
