using BDMS.Shared.DTOs.Auth;
using BDMS.Shared.DTOs.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<Result<ClaimsPrincipal>> AuthenticateAsync(LoginDto dto);
    }
}
