using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using OtanerBank.Models;

namespace OtanerBank.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient http = new HttpClient(); // to call the api later

        public async Task<IActionResult> Index()
        {
            string response = await http.GetStringAsync("https://localhost:44329/Clients");
            List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(response);

            //string response = await client.GetStringAsync("https://localhost:44329/Clients");
            //List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(response);

            return View(Clients.ToList());
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Details(string CPF)
        {
            string response = await http.GetStringAsync("https://localhost:44329/Clients/" + CPF);
            Client client = JsonConvert.DeserializeObject<Client>(response);

            return View(client);

        }

        public IActionResult Edit()
        {
            return View("Edit");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
