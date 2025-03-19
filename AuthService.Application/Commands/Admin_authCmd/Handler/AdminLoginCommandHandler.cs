using AuthService.Application.Common.DTOs.AdminDTOs;
using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Admin_authCmd.Handler
{
    public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand,AdminLoginResDto>
    {
        private readonly IAdminRepo _adminRepo;
        private readonly ICommonService _commonService;

        public AdminLoginCommandHandler(IAdminRepo adminRepo,ICommonService commonService)
        {
            _adminRepo = adminRepo;
            _commonService = commonService;
        }

        public async Task<AdminLoginResDto> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var admins = await _adminRepo.GetAdmins();
                var admin = admins.FirstOrDefault(a=>a.Email==request.Email);

                if (admin==null)
                {
                    throw new ValidationException("not found");
                }

                bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, admin.Password);
                if (!isValidPassword)
                {
                    throw new ValidationException("Invalid Password");
                }

                string JWTtoken = GenerateJwtToken(admin);
                string refresh_token = GenerateRefreshToken();

                var newRefreshEntity = new RefreshToken
                {
                    userId = admin.Id,
                    Refresh_token = refresh_token,
                    Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(7)),
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                    Created_by = admin.Email,
                    Updated_by = admin.Email,
                };


                await _commonService.AddResfreshTokenAsync(newRefreshEntity);

                var res = new AdminLoginResDto()
                {
                    Id=admin.Id,
                    Email = request.Email,
                    Access_token = JWTtoken,
                    Refresh_token = refresh_token,
                };

                return res;

            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        private string GenerateJwtToken(Admin admin)
        {
            if (admin == null)
            {
                throw new UnauthorizedAccessException("Admin not found.");
            }

            var securityKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrEmpty(securityKey))
            {
                throw new Exception("Jwt Secret key is Missing");
            }

            var key = Encoding.UTF8.GetBytes(securityKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
               {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.Name, admin.Email),
                    new Claim(ClaimTypes.Role,"Admin")
               };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
