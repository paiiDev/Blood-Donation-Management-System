using BDMS.Shared.DTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Domain.Interfaces
{
    public interface IBookingService
    {
        Task<Result<bool>> CheckDailyAvailability(int centerId, DateTime date);
    }
}
