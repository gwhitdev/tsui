using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tsui.Models;
using tsui.Interfaces;
using tsui.DataModels;

namespace tsui.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UsersController(ILoggerFactory loggerFactory, IUserService userService)
        {
            _userService = userService;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<UserDataModel> users = await _userService.GetAllUsers();

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
    }
}
