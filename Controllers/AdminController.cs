using EstateServiceApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EstateServiceApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using EstateServiceApp.Models;

namespace EstateServiceApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : BaseController<PropertyModel>
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public AdminController(IAdminRepository adminRepository, IHubContext<NotificationHub> notificationHubContext)
        {            
            _adminRepository = adminRepository;
            _notificationHubContext = notificationHubContext;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            ViewBag.UsersOnline = ConnectedUsers.Ids.Count();
            ViewBag.TotalProperties = (await _adminRepository.GetAllProperties()).Count;
            ViewBag.TotalPhotos = _adminRepository.GetTotalPhotosCount();
            ViewBag.MinPrice = _adminRepository.GetMinPropertyPrice();
            ViewBag.MaxPrice = _adminRepository.GetMaxPropertyPrice();
            ViewData["bedrooms"] = _adminRepository.GetTotalBedRooms();
            ViewData["bathrooms"] = _adminRepository.GetTotalBathRooms();
            ViewData["parking"] = _adminRepository.PropertiesHavingParking();
            ViewData["pool"] = _adminRepository.PropertiesHavingPool();

            var data = await _adminRepository.GetAllProperties();
            var paginatedResult = PaginatedResult(data, page, 5);

            return View(paginatedResult);
        }
        [Route("admin/activity")]
        public async Task<IActionResult> UserActivity()
        {
            return View(await _adminRepository.GetAllUserActivities());
        }

        [Route("admin/json")]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetJson()
        {
            try
            {
                return Ok(await _adminRepository.PieChartData());
            }catch
            {
                return BadRequest();
            }
        }
        [Route("admin/statistics")]
        public async Task<IActionResult> GetAdminStatistics()
        {
            try
            {
                return View( await _adminRepository.PieChartData());
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        public JsonResult DeleteActivity(int Id)
        {
            try
            {
                bool result = false;
                result = _adminRepository.DeleteActivity(Id);
                return Json(result);
            }
            catch
            {
                return null;
            }
        }
    }
}
