using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCEngiBeering.Classes;
using MVCEngiBeering.Data;
using MVCEngiBeering.ViewModels;

namespace MVCEngiBeering.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILogger<LoginController> _logger1;
        private MvcEngibeeringContext _mvcEngibeeringContext;
        public LoginController(ILogger<LoginController> logger1, MvcEngibeeringContext mvcEngibeeringContext)
        {
            _logger1 = logger1;
            _mvcEngibeeringContext = mvcEngibeeringContext;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(LoginViewModel() );
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    private List<UserViewModel> LoginViewModel()


        {
        List<UserViewModel> machines1 = new List<UserViewModel>();

        { 
            UserViewModel temp1 = new UserViewModel
            { 
                userid = 
                username = machines1.username,
                password = machines1.password


            };

            machines1.Add(temp1);


        }

        return machines1;
    }
}
