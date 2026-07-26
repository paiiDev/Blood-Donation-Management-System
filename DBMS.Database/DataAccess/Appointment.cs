using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class Appointment
{
    public int Id { get; set; }

    public string BookingNumber { get; set; } = null!;

    public int DonorId { get; set; }

    public int CenterId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public string TimeSlot { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual DonationCenter Center { get; set; } = null!;

    public virtual ICollection<DonationRecord> DonationRecords { get; set; } = new List<DonationRecord>();

    public virtual Donor IdNavigation { get; set; } = null!;
}
