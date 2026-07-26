using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class BloodInventory
{
    public string Id { get; set; } = null!;

    public int DonationRecordId { get; set; }

    public int BoodGroupId { get; set; }

    public DateOnly Expiry { get; set; }

    public string Status { get; set; } = null!;

    public virtual BloodType BoodGroup { get; set; } = null!;

    public virtual DonationRecord DonationRecord { get; set; } = null!;
}
