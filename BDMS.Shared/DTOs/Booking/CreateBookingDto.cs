using BDMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDMS.Shared.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int CenterId { get; set; }

        [Required]
        public DateTime? AppointmentDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TimeSlotEnum TimeSlot { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(20), RegularExpression(@"^(09|\+?959)\d{7,9}$")]
        public string Phone { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(10)]
        public string BloodGroup { get; set; }
    }
}
