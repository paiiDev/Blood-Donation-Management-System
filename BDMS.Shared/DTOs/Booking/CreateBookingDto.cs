using BDMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDMS.Shared.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int CenterId { get; set; }
        public DateTime? AppointmentDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TimeSlotEnum TimeSlot { get; set; } 

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
    }
}
