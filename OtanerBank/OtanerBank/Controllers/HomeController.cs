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
        public ActionResult Login(string email, string password)
        {
            Admin adm = new Admin
            {
                EMAIL = "thaleslimadejesus@gmail.com",
                PASSWORD = "123",
                NAME = "Thales Lima"
            };

            if (email == "thales" && password == "123")
            {
                adm.PASSWORD = null;

                var admJson = JsonConvert.SerializeObject(adm);
                HttpContext.Session.SetString("AdminLogged", admJson);

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["ErrorLoginMessage"] = "Something went wrong, please try again";
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
