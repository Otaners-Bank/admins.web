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
        static HttpClient client = new HttpClient(); // to call the api later

        public IActionResult Index()
        {
            Task<String> response = client.GetStringAsync("https://localhost:44329/Clients"); // This send a GET request to API and return the body as a String in 'response.Result'
            string result = response.Result;

            List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(result);

            return View(Clients.ToList());
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
