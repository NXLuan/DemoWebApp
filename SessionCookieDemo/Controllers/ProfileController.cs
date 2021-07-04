using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SessionCookieDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionCookieDemo.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {

            UserModel user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            return View(user);
        }
    }
}
