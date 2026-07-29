using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class DonationCenter
{
    public int Id { get; set; }

    public string CenterName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int MaxCapacityPerDay { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<SystemAdmin> SystemAdmins { get; set; } = new List<SystemAdmin>();
}
