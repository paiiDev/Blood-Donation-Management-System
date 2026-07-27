using BDMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Management_System.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> checkAvailability(int centerId, DateTime date)
        {
            var result = await _bookingService.CheckDailyAvailabilityAsync(centerId, date);
            if (!result.IsSuccess)
            {
                return Json(new { isAvailable = false, message = result.ErrorMessage });
            }
            return Json(new { isAvailable = true, message = "ရက်ချိန်းရယူ၍ ရပါသည်။" });
        }
       
    }
}
