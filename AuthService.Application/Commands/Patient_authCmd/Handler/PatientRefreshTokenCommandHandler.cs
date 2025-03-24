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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Patient_authCmd.Handler
{
    public class PatientRefreshTokenCommandHandler : IRequestHandler<PatientRefreshTokenCommand, RefreshTokenResDto>
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ICommonService _commonService;
        public PatientRefreshTokenCommandHandler(IPatientRepo patientRepo,ICommonService commonService)
        {
            _patientRepo = patientRepo;
            _commonService = commonService;
        }
        public async Task<RefreshTokenResDto> Handle(PatientRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Rtoken))
                {
                    throw new UnauthorizedAccessException("Refresh token is required.");
                }

                var dbRToken= await _commonService.GetByTokenAsync(request.Rtoken);

                if (dbRToken == null || dbRToken.Expires < DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException("Unauthorized! Refresh token is invalid or expired.");
                }

                var patient = await _patientRepo.GetPatientById(dbRToken.userId);

                if (patient == null)
                {
                    throw new ValidationException("Agent not found.");
                }

                var newAccessToken = GenerateJwtToken(patient);
                var newRefreshToken = GenerateRefreshToken();

                dbRToken.Refresh_token = newRefreshToken;
                dbRToken.Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(7));
                dbRToken.Updated_on = DateTime.UtcNow;
                dbRToken.Updated_by = patient.Patient_name;

                await _commonService.SaveAsync();

                var res = new RefreshTokenResDto
                {
                    Access_token=newAccessToken,
                    Refresh_token=newRefreshToken,
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


        private string GenerateJwtToken(Patient? patient)
        {
            if (patient == null)
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
                new Claim(ClaimTypes.NameIdentifier, patient.Patient_id.ToString()),
                new Claim(ClaimTypes.Name, patient.Email),
                new Claim(ClaimTypes.Role,"Patient")
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
