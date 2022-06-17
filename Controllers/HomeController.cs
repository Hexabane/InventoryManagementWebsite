using FarmCentralApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//HomeController class where the homepage and its razor views have all the backend code.
namespace FarmCentralApp.Controllers
{//start of namespace

    public class HomeController : Controller
    {//start of public class

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //The method that displays the index razor view with all the necessary information beisides hardcoded html.
        public IActionResult Index()
        {//start of Index()

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = UsersController.employee;
            ViewBag.roleFarmer = UsersController.farmer;

            //If the user tries to access any features of the website without logging in.
            if (UsersController.usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }
           
            return View();

        }//end of Index()

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }//end of public class

}//end of namespace
