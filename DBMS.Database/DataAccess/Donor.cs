using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class Donor
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? BloodGroupType { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ICollection<DonationRecord> DonationRecords { get; set; } = new List<DonationRecord>();
}
