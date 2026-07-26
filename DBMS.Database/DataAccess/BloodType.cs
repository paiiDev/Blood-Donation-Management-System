using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class BloodType
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<BloodInventory> BloodInventories { get; set; } = new List<BloodInventory>();

    public virtual ICollection<DonationRecord> DonationRecords { get; set; } = new List<DonationRecord>();
}
