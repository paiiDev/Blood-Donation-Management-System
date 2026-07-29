using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Database.DataAccess
{
    public partial class AppDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Donor>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<SystemAdmin>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<DonationCenter>().HasQueryFilter(dc => !dc.IsDeleted);
        }
    }
}
