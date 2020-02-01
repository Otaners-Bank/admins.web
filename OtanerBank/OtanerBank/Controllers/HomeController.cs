﻿using System;
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

        public IActionResult Login()
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
        public async Task<IActionResult> Login(string CPF, string PASSWORD)
        {
            Admin adm = new Admin
            {
                CPF = CPF,
                PASSWORD = PASSWORD
            };

            HttpClient http = new HttpClient();

            var jsonString = JsonConvert.SerializeObject(adm);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var message = http.PostAsync("https://localhost:44329/Admins/Login", httpContent);

            adm = null;

            if (message.Result.IsSuccessStatusCode)
            {
                string response = await http.GetStringAsync("https://localhost:44329/Admins/" + CPF);
                Admin admin = JsonConvert.DeserializeObject<Admin>(response);

                var admJson = JsonConvert.SerializeObject(admin);
                HttpContext.Session.SetString("AdminLogged", admJson);
                

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["ErrorLoginMessage"] = message.Result.StatusCode;
                return View();
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

    }
}
