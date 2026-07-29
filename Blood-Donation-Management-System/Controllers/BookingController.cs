using BDMS.Domain.Interfaces;
using BDMS.Shared.DTOs.Booking;
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

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return Json(new { success = false, message = "အချက်အလက်များ ပြည့်စုံမှုမရှိပါ။ ပြန်လည်စစ်ဆေးပေးပါ။" });
            }

            var result = await _bookingService.CreateBookingAsync(dto);
            if (!result.IsSuccess)
            {
                return Json(new { isSuccess = false, message = result.ErrorMessage });
            }
            return Json(new { isSuccess = true, message = "Booking တင်ခြင်း အောင်မြင်ပါသည်။" });

        }
    }
}
