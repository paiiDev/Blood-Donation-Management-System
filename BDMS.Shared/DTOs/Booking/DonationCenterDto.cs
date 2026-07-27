using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Shared.DTOs.Booking
{
    public class DonationCenterDto
    {
        public int Id { get; set; }

        public string CenterName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int MaxCapacityPerDay { get; set; }
    }
}
