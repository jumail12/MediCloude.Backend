using AuthService.Application.Common.DTOs.CommonDtos;
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
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Admin_authCmd.Handler
{
    public class AdminRefreshTokenCommandHandler : IRequestHandler<AdminRefreshTokenCommand, RefreshTokenResDto>
    {
        private readonly IAdminRepo _adminRepo;
        private readonly ICommonService _commonService;
        public AdminRefreshTokenCommandHandler(IAdminRepo adminrepo, ICommonService commonService)
        {
            _adminRepo = adminrepo;
            _commonService = commonService;
        }
        public async Task<RefreshTokenResDto> Handle(AdminRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Rtoken))
                {
                    throw new UnauthorizedAccessException("Refresh token is required.");
                }

                var dbRToken = await _commonService.GetByTokenAsync(request.Rtoken);

                if (dbRToken == null || dbRToken.Expires < DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException("Unauthorized! Refresh token is invalid or expired.");
                }

                var admin = await _adminRepo.GetByIdAsync(dbRToken.userId);

                var newAccessToken = GenerateJwtToken(admin);
                var newRefreshToken = GenerateRefreshToken();

                dbRToken.Refresh_token = newRefreshToken;
                dbRToken.Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(7));
                dbRToken.Updated_on = DateTime.UtcNow;
                dbRToken.Updated_by = admin.Email;

                await _commonService.SaveAsync();

                var res = new RefreshTokenResDto
                {
                    Access_token = newAccessToken,
                    Refresh_token = newRefreshToken,
                };

                return res;

            }
            catch (UnauthorizedAccessException)
            {
                throw;
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
