using EstateServiceApp.Data;
using EstateServiceApp.Models;
using EstateServiceApp.Repository;
using EstateServiceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IPropertyRepository _propertyRepository;
        public HomeController(ILogger<HomeController> logger,IUserService userService,IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _userService = userService;
            _propertyRepository = propertyRepository;
        }
        /// <summary>
        /// This method redirects to home page of the application 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var isLoggedin = _userService.IsAuthenticated();
            return View(await _propertyRepository.GetProperties());

        }
        /// <summary>
        /// This method implements search action of properties in home page based on filters specified.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="typeValue"></param>
        /// <param name="address"></param>
        /// <param name="minBudget"></param>
        /// <param name="maxBudget"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(int search,PropertyStatusEnum typeValue, string address,int minBudget,int maxBudget)
        {
            ViewData["id"] = search;
            ViewData["type"] = typeValue;
            ViewData["address"] = address;
            ViewData["min"] = minBudget;
            ViewData["max"] = maxBudget;

            var query = await _propertyRepository.GetTop(search,typeValue,address, minBudget, maxBudget);
            return View(query);
        }

        public IActionResult AboutUs()
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
