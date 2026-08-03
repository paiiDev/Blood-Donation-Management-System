using DBMS.Database.DataAccess;
using DBMS.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Database.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SystemAdmin?> GetByUsernameAsync(string username)
        {
            return await _context.SystemAdmins.AsNoTracking().FirstOrDefaultAsync(a => a.Username == username);

        }
    }
}
