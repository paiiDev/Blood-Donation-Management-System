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
    }
}
