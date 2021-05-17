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
    public class volunteersController : Controller
    {
        private readonly IVolunteerService _volunteerService;
        private readonly ILogger _logger;
        private string AccessToken { get; set; }

        public volunteersController(ILoggerFactory loggerFactory, IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
            _logger = loggerFactory.CreateLogger<volunteersController>();
        }

        public async void GetAccessToken()
        {
            AccessToken = await HttpContext.GetTokenAsync("access_token");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            List<VolunteerDataModel> volunteers = await _volunteerService.GetAllVolunteersAsync();

            VolunteerViewModel volunteerViewModel = new()
            {
                VolunteerIds = new List<string>()
            };

            if (volunteers.Count > 0)
            {
                try
                {
                    _logger.LogInformation("Trying to add volunteerIds to list...");
                    foreach (var u in volunteers)
                    {
                        volunteerViewModel.VolunteerIds.Add(u.Id);
                    }
                    _logger.LogInformation("Successfully created list");
                    return View(volunteerViewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return View(volunteerViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("Volunteers/{volunteerId}")]
        public async Task<IActionResult> Info(string volunteerId)
        {
            if (!User.Identity.IsAuthenticated) throw new UnauthorizedAccessException();

            GetAccessToken();
            VolunteerViewModel volunteerViewModel = new();

            try
            {
                _logger.LogInformation("Trying to get volunteer information...");
                var volunteerData = await _volunteerService.GetVolunteerAsync(volunteerId);
                foreach (var d in volunteerData)
                {
                    volunteerViewModel.Id = d.Id;
                    volunteerViewModel.UpdatedAt = d.UpdatedAt;
                }
                _logger.LogInformation("Successfully requested volunteer info.");
                return View(volunteerViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View(volunteerViewModel);

        }
    }
}
