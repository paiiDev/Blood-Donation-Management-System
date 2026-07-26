using System;
using System.Collections.Generic;

namespace DBMS.Database.DataAccess;

public partial class SystemAdmin
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int CenterId { get; set; }

    public virtual DonationCenter Center { get; set; } = null!;
}
