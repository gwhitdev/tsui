using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using tsui.Models;
using tsui.Interfaces;
using tsui.DataModels;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace tsui.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private string AccessToken { get; set; }

        public UsersController(ILoggerFactory loggerFactory, IUserService userService)
        {
            _userService = userService;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        public async void GetAccessToken()
        {
            AccessToken = await HttpContext.GetTokenAsync("access_token");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;
            var createLocalUserIfNotExists = await _userService.CreateUser(AccessToken, userId);
            List<UserDataModel> users = await _userService.GetAllUsersAsync();

            UserViewModel userViewModel = new()
            {
                UserIds = new List<string>()
            };
            
            if(users.Count > 0)
            {
                try
                {
                    _logger.LogInformation("Trying to add userIds to list...");
                    foreach (var u in users)
                    {
                        userViewModel.UserIds.Add(u.Id);
                    }
                    _logger.LogInformation("Successfully created list");
                    return View(userViewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return View(userViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("/Users/{userId}")]
        public async Task<IActionResult> Info(string userId)
        {
            if (!User.Identity.IsAuthenticated) throw new UnauthorizedAccessException();

            GetAccessToken();
            UserViewModel userViewModel = new();

            try
            {
                _logger.LogInformation("Trying to get user information...");
                var userData =  await _userService.GetUserAsync(userId);
                foreach(var d in userData)
                {
                    userViewModel.Id = d.Id;
                    userViewModel.UpdatedAt = d.UpdatedAt;
                }
                _logger.LogInformation("Successfully requested user info.");
                return View(userViewModel);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(userViewModel);

        }
    }
}
