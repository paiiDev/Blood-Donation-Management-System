using DBMS.Database.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Database.Interfaces
{
    public interface IAuthRepository
    {
        Task<SystemAdmin?> GetByUsernameAsync(string username);
    }
}
