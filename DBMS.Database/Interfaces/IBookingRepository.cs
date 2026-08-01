using BDMS.Shared.DTOs.Booking;
using DBMS.Database.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Database.Interfaces
{
    public interface IBookingRepository
    {
        Task<AvailabilitySPResult> GetDailyAvailabilityAsync(int centerId, DateTime date);

        Task<IEnumerable<DonationCenter>> GetAllDonationCentersAsync();

        Task<int?> GetBloodGroupIdByNameAsync(string groupName);

        Task<Donor> GetDonorByPhoneAsync(string phone);
        Task<bool> HasPendingBookingAsync(string phone, string email);

        Task<bool> SaveBookingTransactionAsync(Donor newDonor, int? existingDonor, Appointment appointment);


    }
}
