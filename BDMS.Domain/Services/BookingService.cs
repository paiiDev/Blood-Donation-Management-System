using BDMS.Domain.Interfaces;
using BDMS.Shared.DTOs.Booking;
using BDMS.Shared.DTOs.Result;
using DBMS.Database.DataAccess;
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

        public async Task<Result<bool>> CheckPendingDonor(string phone, string email)
        {
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email))
            {
                return Result<bool>.Failure("ဖုန်းနံပါတ် သို့မဟုတ် အီးမေးလ် ထည့်သွင်းရန် လိုအပ်ပါသည်။");
            }

            bool hasPendingBooking = await _bookingRepo.HasPendingBookingAsync(phone, email);

            if (hasPendingBooking)
            {
                return Result<bool>.Failure("လူကြီးမင်း၏ ယခင်စာရင်းသွင်းထားမှုမှာ အတည်ပြုရန် စောင့်ဆိုင်းဆဲ (Pending) အဆင့်တွင် ရှိနေဆဲဖြစ်သဖြင့် အသစ်ထပ်မံစာရင်းသွင်း၍ မရနိုင်သေးပါ။");
            }
            else
            {
                return Result<bool>.Success(true);
            }
        }

        public async Task<Result<bool>> CreateBookingAsync(CreateBookingDto dto)
        {
      
            var bloodGroupId = await _bookingRepo.GetBloodGroupIdByNameAsync(dto.BloodGroup);
            if(bloodGroupId == null)
            {
                return Result<bool>.Failure("သွေးအုပ်စု မှားယွင်းနေပါသည်။");
            }

            var existingDonor = await _bookingRepo.GetDonorByPhoneAsync(dto.Phone);

            Donor? donorToSave = null;
            int? existingDonorId = null;

            if (existingDonor != null)
            {
                existingDonorId = existingDonor.Id;
            } else
            {
                donorToSave = new Donor
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    BloodGroupType = bloodGroupId.Value
                };
            }

            string bookingNumber = "BK-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();

            var appointment = new Appointment
            {
                CenterId = dto.CenterId,
                AppointmentDate = DateOnly.FromDateTime(dto!.AppointmentDate!.Value),
                TimeSlot = dto.TimeSlot.ToString(),
                BookingNumber = bookingNumber,
                Status = "Pending"
            };

            var isSaved = await _bookingRepo.SaveBookingTransactionAsync(donorToSave!, existingDonorId, appointment);

            if (isSaved)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("စာရင်းသွင်းရာတွင် အမှားအယွင်းရှိပါသည်။");
        }
    }
}
