using BDMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Domain.Helpers
{
    public static class TimeSlotHelper
    {
        public static string GetTimeSlotDescription(TimeSlotEnum slot)
        {
            return slot switch
            {
                TimeSlotEnum.T1 => "09:00 AM - 10:00 AM",
                TimeSlotEnum.T2 => "10:00 AM - 11:00 AM",
                TimeSlotEnum.T3 => "01:00 PM - 02:00 PM",
                _ => "အချိန် သတ်မှတ်ထားခြင်း မရှိပါ"
            };
        }
    }
}
