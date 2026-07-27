using BDMS.Domain.Interfaces;
using BDMS.Shared.DTOs.Booking;
using BDMS.Shared.DTOs.Result;
using DBMS.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        public BookingService(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        public async Task<Result<bool>> CheckDailyAvailabilityAsync(int centerId, DateTime date)
        {
            if( date.Date < DateTime.Now.Date)
            {
                return Result<bool>.Failure("Cannot select previous date for booking.");
            }

            var result = await _bookingRepo.GetDailyAvailabilityAsync(centerId, date);
            if( result.IsAvailable)
            {
                return Result<bool>.Success(true);
            } else
            {
                return Result<bool>.Failure(result.Message);

            }

        }

        public async Task<Result<IEnumerable<DonationCenterDto>>> GetAllDonationCentersAsync()
        {
            var centers = await _bookingRepo.GetAllDonationCentersAsync();
            var centerDtos = centers.Select(c => new DonationCenterDto
            {
                Id = c.Id,
                CenterName = c.CenterName,
                Address = c.Address,
                MaxCapacityPerDay = c.MaxCapacityPerDay
            });
            return Result<IEnumerable<DonationCenterDto>>.Success(centerDtos);
        }
    }
}
