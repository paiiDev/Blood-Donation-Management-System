using BDMS.Shared.DTOs.Booking;
using Dapper;
using DBMS.Database.DataAccess;
using DBMS.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Database.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AvailabilitySPResult> GetDailyAvailabilityAsync(int centerId, DateTime date)
        {
            using var connection = _context.Database.GetDbConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@CenterId", centerId);
            parameters.Add("@AppointmentDate", date.Date);

            var spResult = await connection.QueryFirstOrDefaultAsync<AvailabilitySPResult>("CheckDailyAvailability", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return spResult ?? new AvailabilitySPResult { IsAvailable = false, Message = "Error, canno't find availibality." };
        } 

        public async Task<IEnumerable<DonationCenter>> GetAllDonationCentersAsync(){
            return await _context.DonationCenters.ToListAsync();
        }

    }
}
