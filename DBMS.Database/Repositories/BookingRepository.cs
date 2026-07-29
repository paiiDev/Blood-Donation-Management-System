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

        public async Task<int?> GetBloodGroupIdByNameAsync(string groupName)
        {
            var bloodGroup = await _context.BloodTypes.FirstOrDefaultAsync(bg => bg.GroupName == groupName);
            return bloodGroup?.Id;
        }

        public async Task<Donor> GetDonorByPhoneAsync(string phone)
        {
            return await _context.Donors.FirstOrDefaultAsync(d => d.Phone == phone);
        }

        public async Task<bool> SaveBookingTransactionAsync(Donor newDonor, int? existingDonorId, Appointment appointment)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (newDonor != null)
                {
                    _context.Donors.Add(newDonor);
                    await _context.SaveChangesAsync();

                    appointment.DonorId = newDonor.Id;
                } else
                {
                    appointment.DonorId = existingDonorId!.Value;
                }

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}