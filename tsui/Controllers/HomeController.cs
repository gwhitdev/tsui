using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using tsui.Models;
using System.Globalization;

namespace tsui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string AccessToken { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async void GetAccessToken()
        {
            AccessToken = await HttpContext.GetTokenAsync("access_token");
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                GetAccessToken();
                DateTime accessTokenExpiresAt = DateTime.Parse(
                        await HttpContext.GetTokenAsync("expires_at"),
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind);

                string idToken = await HttpContext.GetTokenAsync("id_token");
            }
            return View();
        }

        public IActionResult Privacy()
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
