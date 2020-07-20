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
    public class HomeController : Controller
    {

        string ip = "http://177.71.131.138";
        // https://localhost:44329 = Local
        // 177.71.131.138 = AWS Instance

        public IActionResult Index()
        {
            if (authorized())
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Signin()
        {
            if (authorized())
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(string EMAIL, string PASSWORD)
        {
            Admin adm = new Admin
            {
                EMAIL = EMAIL,
                PASSWORD = PASSWORD
            };

            HttpClient http = new HttpClient();

            var jsonString = JsonConvert.SerializeObject(adm);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await http.PostAsync(ip + "/Admins/Login", httpContent);

            adm = null;

            if (message.IsSuccessStatusCode)
            {
                ViewData["ErrorLoginMessage"] = "";
                string response = await http.GetStringAsync(ip + "/Admins/Search/Login/" + EMAIL);
                Admin admin = JsonConvert.DeserializeObject<Admin>(response);

                var admJson = JsonConvert.SerializeObject(admin);
                HttpContext.Session.SetString("AdminLogged", admJson);


                return RedirectToAction("Index", "Admin");
            }
            else
            {
                int statusCode = (int)message.StatusCode;

                switch (statusCode)
                {
                    case 404:
                        ViewData["ErrorLoginMessage"] = "Account not found";
                        break;
                    case 400:
                        ViewData["ErrorLoginMessage"] = "Invalid credentials";
                        break;
                    default:
                        ViewData["ErrorLoginMessage"] = "An error occurred";
                        break;
                }

                return View("Index");
            }

        }

        private bool authorized()
        {
            if (HttpContext.Session.GetString("AdminLogged") == null)
            {
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
            if (authorized())
            {
                return RedirectToAction("Login", "Home");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
