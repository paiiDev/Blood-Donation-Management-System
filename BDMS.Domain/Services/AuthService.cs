using BCrypt.Net;
using BDMS.Domain.Interfaces;
using BDMS.Shared.DTOs.Auth;
using BDMS.Shared.DTOs.Result;
using DBMS.Database.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BDMS.Domain.Services
{
    public class AuthService : IAuthService
    { 
        private readonly IAuthRepository _authRepo;
        public AuthService(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        public async Task<Result<ClaimsPrincipal>> AuthenticateAsync(LoginDto dto)
        {
            if (dto is null)
                return Result<ClaimsPrincipal>.Failure("Username and password are required.");

            var user = await _authRepo.GetByUsernameAsync(dto.Username.Trim());
            if(user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Result<ClaimsPrincipal>.Failure("Invalid username or password.");
            }

            var claims = new List<Claim>
             {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
             };

            if (user.CenterId.HasValue)
            {
                claims.Add(new Claim("CenterId", user.CenterId.Value.ToString()));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return Result<ClaimsPrincipal>.Success(claimsPrincipal);
        }

    }
    
}
