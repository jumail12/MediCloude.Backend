using AuthService.Application.Common.DTOs.DoctorDTOs;
using AuthService.Application.Common.DTOs.PatientDTOs;
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

namespace AuthService.Application.Commands.Doctor_authCmd.Handler
{
    public class DrLoginCommandHandler : IRequestHandler<DrLoginCommand, DrLoginResDto>
    {
        private readonly IDrRepo _repo;
        private readonly ICommonService _commonService;
        public DrLoginCommandHandler(IDrRepo repo, ICommonService commonService)
        {
            _repo = repo;
            _commonService = commonService;
        }

        public async Task<DrLoginResDto> Handle(DrLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allDrs=await _repo.GetAllDrs();
                var dr= allDrs.FirstOrDefault(a=>a.Email==request.Email);

                if (dr == null)
                {
                    throw new ValidationException("not found");
                }

                bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, dr.Password);
                if (!isValidPassword)
                {
                    throw new ValidationException("Invalid Password");
                }

                if (!dr.Is_approved || dr.Is_blocked)
                {
                    if (!dr.Is_approved)
                    {
                        throw new UnauthorizedAccessException("Your license verification could not be completed. Please check your credentials or contact support for assistance.");
                    }
                    else
                    {
                        throw new Exception("Your account is currently blocked. Please contact support for assistance.");
                    }
                }


                string JWTtoken = GenerateJwtToken(dr);
                string refresh_token = GenerateRefreshToken();

                var newRefreshEntity = new RefreshToken
                {
                    userId = dr.Id,
                    Refresh_token = refresh_token,
                    Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(7)),
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                    Created_by = dr.Doctor_name,
                    Updated_by = dr.Doctor_name,
                };

                await _commonService.AddResfreshTokenAsync(newRefreshEntity);

                var loginRes = new DrLoginResDto
                {
                    Id = dr.Id,
                    Email=dr.Email,
                    Doctor_name = dr.Doctor_name,
                    Refresh_token = refresh_token,
                    Access_token = JWTtoken,
                };

                return loginRes;

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

        private string GenerateJwtToken(Doctor? dr)
        {
            if (dr == null)
            {
                throw new Exception("Patient not found.");
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
                new Claim(ClaimTypes.NameIdentifier, dr.Id.ToString()),
                new Claim(ClaimTypes.Name, dr.Email),
                new Claim(ClaimTypes.Role,"Doctor")
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
