using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OtanerBank.Models;

namespace OtanerBank.Controllers
{
    public class AdminController : Controller
    {
        static HttpClient http = new HttpClient(); // to call the api later

        // -- Dashboard

        public async Task<IActionResult> Index()
        {
            try
            {
                if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/Search/All");
                List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(response);

                return View(Clients.ToList());

            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public async Task<string> CountActives()
        {
            string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/Active/CountTotal");
            return response;
        }

        [HttpGet]
        public async Task<string> CountInactives()
        {
            string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/Inactive/CountTotal");
            return response;
        }

        public async Task<ActionResult> Inactive(string CPF)
        {
            HttpResponseMessage response = await http.DeleteAsync("https://localhost:44329/Admins/Clients/InactiveClient/" + CPF);
            return RedirectToAction("Index","Admin");
        }
        
        public async Task<ActionResult> Active(string CPF)
        {
            HttpResponseMessage response = await http.DeleteAsync("https://localhost:44329/Admins/Clients/ActiveClient/" + CPF);
            return RedirectToAction("Index","Admin");
        }

        // -- Dashboard end

        private bool AdminUnauthorized()
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLogged") != null)
                {
                    Admin adm = JsonConvert.DeserializeObject<Admin>(HttpContext.Session.GetString("AdminLogged"));

                    Task<string> response = http.GetStringAsync("https://localhost:44329/Admins/Search/" + adm.CPF);

                    Admin admin = JsonConvert.DeserializeObject<Admin>(response.Result);

                    if (adm.PASSWORD != null && adm.PASSWORD == admin.PASSWORD)
                    {
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
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }

        }

        public async Task<IActionResult> Details(string CPF)
        {
            try
            {
                if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/Search/" + CPF);
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
                if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/Search/" + CPF);
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
                if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

                string response = await http.GetStringAsync("https://localhost:44329/Admins/Clients/Search/" + client.CPF);
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
                var message = await http.PutAsync("https://localhost:44329/Admins/Clients/Update/" + client.CPF, httpContent);

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
                if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

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
                if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

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
                var message = await http.PostAsync("https://localhost:44329/Admins/Clients/Register/", httpContent);

                return RedirectToAction("Index"); // and returns to the Home Page
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<bool> Profile(Admin admin)
        {
            if (AdminUnauthorized()) return false;

            try
            {
                if (String.IsNullOrWhiteSpace(admin.CPF) || String.IsNullOrEmpty(admin.CPF)
                    || String.IsNullOrWhiteSpace(admin.NAME) || String.IsNullOrEmpty(admin.NAME)
                    || String.IsNullOrWhiteSpace(admin.PASSWORD) || String.IsNullOrEmpty(admin.PASSWORD))
                {
                    return false;
                }
                else
                {
                    var jsonString = JsonConvert.SerializeObject(admin); // Serializing object to put in the JsonObject
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var message = await http.PutAsync("https://localhost:44329/Admins/Update/" + ViewData["CurrentAdminCPF"], httpContent);


                    Task<string> response = http.GetStringAsync("https://localhost:44329/Admins/Search/" + admin.CPF);
                    Admin adm = JsonConvert.DeserializeObject<Admin>(response.Result);

                    ViewData["CurrentAdminId"] = admin.id;
                    ViewData["CurrentAdminCPF"] = admin.CPF;
                    ViewData["CurrentAdminName"] = adm.NAME;
                    ViewData["CurrentAdminEmail"] = adm.EMAIL;
                    ViewData["CurrentAdminPassword"] = adm.PASSWORD;

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogged");
            ViewData["CurrentAdminId"] = null;
            ViewData["CurrentAdminCPF"] = null;
            ViewData["CurrentAdminName"] = null;
            ViewData["CurrentAdminEmail"] = null;
            ViewData["CurrentAdminPassword"] = null;
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (AdminUnauthorized()) return RedirectToAction("Index", "Home");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}