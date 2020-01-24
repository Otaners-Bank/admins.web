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
            catch
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
            catch
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
            catch
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
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> SaveUpdates(Client client)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(client); // Serializing object to put in the JsonObject
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var message = await http.PutAsync("https://localhost:44329/Clients/" + client.CPF, httpContent);

                return RedirectToAction("Index"); // and returns to the Home Page
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewClient(Client client)
        {
            try
            {
                client.BALANCE = "R$ 0.0";
                var jsonString = JsonConvert.SerializeObject(client); // Serializing object to put in the JsonObject
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var message = await http.PostAsync("https://localhost:44329/Clients", httpContent);

                return RedirectToAction("Index"); // and returns to the Home Page
            }
            catch
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
