using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OtanerBank.Models;

namespace OtanerBank.Controllers
{
    public class AdminController : Controller
    {

        static HttpClient http = new HttpClient(); // to call the api later

        [HttpPost]
        public async Task<bool> Profile(Admin admin)
        {
            if (unauthorized())
            {
                RedirectToAction("Login", "Home");
            }

            try
            {
                var jsonString = JsonConvert.SerializeObject(admin); // Serializing object to put in the JsonObject
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var message = await http.PutAsync("https://localhost:44329/Admins/" + ViewData["CurrentAdminCPF"], httpContent);


                Task<string> response = http.GetStringAsync("https://localhost:44329/Admins/" + admin.CPF);
                Admin adm = JsonConvert.DeserializeObject<Admin>(response.Result);

                ViewData["CurrentAdminId"] = admin.id;
                ViewData["CurrentAdminCPF"] = admin.CPF;
                ViewData["CurrentAdminName"] = adm.NAME;
                ViewData["CurrentAdminEmail"] = adm.EMAIL;
                ViewData["CurrentAdminPassword"] = adm.PASSWORD;

                return true;


            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (unauthorized())
                {
                    return RedirectToAction("Login", "Home");
                }

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients");
                List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(response);

                return View(Clients.ToList());

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
                if (unauthorized())
                {
                    return RedirectToAction("Login", "Home");
                }

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/" + CPF);
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
                if (unauthorized())
                {
                    return RedirectToAction("Login", "Home");
                }

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/" + CPF);
                Client client = JsonConvert.DeserializeObject<Client>(response);

                return View(client);
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Client client)
        {
            try
            {
                if (unauthorized())
                {
                    return RedirectToAction("Login", "Home");
                }

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/" + client.CPF);
                Client oldClientInformation = JsonConvert.DeserializeObject<Client>(response);

                client.PASSWORD = oldClientInformation.PASSWORD;
                client.EMAIL = oldClientInformation.EMAIL;

                if (client.MANAGER_NAME == null && client.MANAGER_EMAIL == null)
                {
                    client.MANAGER_NAME = ""; client.MANAGER_EMAIL = "";
                }

                if (client.LAST_ACCESS == null && client.BALANCE_EARNED == null)
                {
                    client.LAST_ACCESS = ""; client.BALANCE_EARNED = "";
                }

                var jsonString = JsonConvert.SerializeObject(client); // Serializing object to put in the JsonObject
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var message = await http.PutAsync("https://localhost:44329/Admins/Clients/" + client.CPF, httpContent);

                return RedirectToAction("Index"); // and returns to the Home Page
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
                if (unauthorized())
                {
                    return RedirectToAction("Login", "Home");
                }

                return View();
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(Client client)
        {
            try
            {
                if (unauthorized())
                {
                    return RedirectToAction("Login", "Home");
                }

                client.BALANCE = "R$ 0.0";

                if (client.MANAGER_NAME == null && client.MANAGER_EMAIL == null)
                {
                    client.MANAGER_NAME = ""; client.MANAGER_EMAIL = "";
                }

                if (client.LAST_ACCESS == null && client.BALANCE_EARNED == null)
                {
                    client.LAST_ACCESS = ""; client.BALANCE_EARNED = "";
                }

                var jsonString = JsonConvert.SerializeObject(client); // Serializing object to put in the JsonObject
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var message = await http.PostAsync("https://localhost:44329/Admins/Clients", httpContent);

                return RedirectToAction("Index"); // and returns to the Home Page
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogged");
            return RedirectToAction("Index", "Home");
        }

        private bool unauthorized(bool updated = false)
        {
            if (HttpContext.Session.GetString("AdminLogged") != null)
            {
                Admin adm = JsonConvert.DeserializeObject<Admin>(HttpContext.Session.GetString("AdminLogged"));

                Task<string> response = http.GetStringAsync("https://localhost:44329/Admins/" + adm.CPF);
                Admin admin = JsonConvert.DeserializeObject<Admin>(response.Result);

                ViewData["CurrentAdminId"] = adm.id;
                ViewData["CurrentAdminCPF"] = adm.CPF;
                ViewData["CurrentAdminName"] = admin.NAME;
                ViewData["CurrentAdminEmail"] = admin.EMAIL;
                ViewData["CurrentAdminPassword"] = admin.PASSWORD;

                return false;
            }
            else
            {
                return true;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (unauthorized())
            {
                return RedirectToAction("Login", "Home");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}