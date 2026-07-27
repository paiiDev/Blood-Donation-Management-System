using System.Diagnostics;
using BDMS.Domain.Interfaces;
using Blood_Donation_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookingService _bookingService;

        public HomeController(ILogger<HomeController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var centersResult = await _bookingService.GetAllDonationCentersAsync();
            if (centersResult == null)
            {
                ViewBag.ErrorMessage = "Failed to retrieve donation centers.";
                return View();
            } else
            {
                ViewBag.DonationCenters = centersResult.Data;
                return View();
            }
             
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
