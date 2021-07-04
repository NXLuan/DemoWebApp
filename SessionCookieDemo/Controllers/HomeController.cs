using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SessionCookieDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SessionCookieDemo.Controllers
{
    public class HomeController : Controller
    {
        // data demo
        Dictionary<string, UserModel> ListUser = new Dictionary<string, UserModel>()
        {
            { "abc", new UserModel(){ Username = "Luan", Password="123"} },
            { "def", new UserModel(){ Username = "Tu", Password="456"} },
            { "ghj", new UserModel(){ Username = "Chuong", Password="789"} }
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // get user id from cookie
            string uid = Request.Cookies["uid"];

            // check if the user is logged in
            if (string.IsNullOrEmpty(uid))
                return RedirectToAction("Login");

            // write session save user
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(ListUser[uid]));
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Status = "Login";
            return View();
        }

        // called when login button pressed
        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            // get user id from data using entered username and password 
            string uid = ListUser.FirstOrDefault(e => e.Value.Username.Equals(user.Username) && e.Value.Password.Equals(user.Password)).Key;

            // check user exists
            if (!string.IsNullOrEmpty(uid))
            {
                // test expire cookie
                //CookieOptions option = new CookieOptions();
                //option.Expires = DateTime.Now.AddMilliseconds(5000);
                //Response.Cookies.Append("uid", uid, option);

                // write cookie save id user
                Response.Cookies.Append("uid", uid);

                // move to home page
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Wrong username or password";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("uid");
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
