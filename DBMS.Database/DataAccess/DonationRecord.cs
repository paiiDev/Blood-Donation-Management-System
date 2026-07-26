using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class DonationRecord
{
    public int Id { get; set; }

    public int AppiontmentId { get; set; }

    public int DonorId { get; set; }

    public int BloodGroupType { get; set; }

    public DateTime DonationDate { get; set; }

    public int Volume { get; set; }

    public virtual Appointment Appiontment { get; set; } = null!;

    public virtual BloodType BloodGroupTypeNavigation { get; set; } = null!;

    public virtual ICollection<BloodInventory> BloodInventories { get; set; } = new List<BloodInventory>();

    public virtual Donor Donor { get; set; } = null!;
}
