using EstateServiceApp.Data;
using EstateServiceApp.Hubs;
using EstateServiceApp.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Models
{
    /// <summary>
    /// This filter keeps track of all customers activity by administrator
    /// </summary>
    public class UserActivityFilter : IActionFilter
    {
        private readonly PropertyDbContext _context = null;
        private readonly IUserService _userService;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public UserActivityFilter(PropertyDbContext context,IUserService userService , IHubContext<NotificationHub> notificationHubContext)
        {
            _context = context;
            _userService = userService;
            _notificationHubContext = notificationHubContext;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        /// <summary>
        /// This method keeps track of authenticated customers visiting each property details page and when they are downloading
        /// its brochure details as well .This table is visible to Admin only.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var contollername = context.RouteData.Values["controller"];
            var actionname = context.RouteData.Values["action"];
            var url = $"{contollername}/{actionname}";
            var propertyId="";
            var userData=new KeyValuePair<string,object>();
            if (!string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
            {
                return;
            }
            else
            {
                userData = context.ActionArguments.FirstOrDefault();
                if (userData.Key != null && userData.Value != null)
                {
                    propertyId = userData.Key;
                }
            }
            if (url == "Property/GetProperty" || url == "Property/DownloadBrochure")
            {
                if (_userService.GetUserId() != null)
                {
                    var userName = _context.Users.Where(x => x.Id == _userService.GetUserId()).FirstOrDefault().FirstName;
                    var role = context.HttpContext.User.IsInRole("Admin");
                    if (role == false)
                    {
                        var email = context.HttpContext.User.Identity.Name;

                        if (propertyId != "")
                        {
                            StoreUserActivity((int)userData.Value, url, userName, email);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Generates custom notification message for admin
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private string NotifyAdminMessage(int propertyId, string url, string userName,string email)
        {
            return "Customer " + email + " with email Id " + userName + " has downloaded brochure for property Id " + propertyId;
        }

        /// <summary>
        /// This table adds new user activity into the database.Also notifies Admin if the customer has 
        /// downloaded a brochure of any property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="url"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        public void StoreUserActivity(int propertyId, string url, string email, string userName)
        {
            var userActivity = new UserActivity
            {
                UserName = email,
                PropertyId = propertyId,
                Url = url,
                UserEmail = userName
            };

            _context.UserActivites.Add(userActivity);
            _context.SaveChanges();
            if (url == "Property/DownloadBrochure")
            {
                var notification = NotifyAdminMessage(propertyId, url, userName, email);
                _notificationHubContext.Clients.All.SendAsync("ReceiveMessage", notification);
            }
        }
    }
}
