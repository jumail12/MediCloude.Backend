using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Common.DTOs.PatientDTOs;
using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthService.Application.Commands.Patient_authCmd.Handler
{
    public class PateintLoginCommandHandler : IRequestHandler<PatientLoginCommand, PatientLoginResDto>
    {
        private readonly IPatientRepo _repo;
        private readonly ICommonService _commonService;
        public PateintLoginCommandHandler(IPatientRepo repo, ICommonService commonService)
        {
            _repo = repo;
            _commonService = commonService;
        }

        public async Task<PatientLoginResDto> Handle(PatientLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allPatients = await _repo.GetAllPatients();
                var patient = allPatients.FirstOrDefault(a => a.Email == request.Email);

                if (patient == null)
                {
                    throw new ValidationException("not found");
                }

                bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, patient.Password);
                if (!isValidPassword)
                {
                    throw new ValidationException("Invalid Password");
                }

                if (patient.Is_blocked == true)
                {
                    throw new UnauthorizedAccessException("Your account is Blocked");
                }

                string JWTtoken = GenerateJwtToken(patient);
                string refresh_token = GenerateRefreshToken();

                var newRefreshEntity = new RefreshToken
                {
                    userId = patient.Patient_id,
                    Refresh_token = refresh_token,
                    Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(7)),
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                    Created_by = patient.Patient_name,
                    Updated_by = patient.Patient_name,
                };

                await _commonService.AddResfreshTokenAsync(newRefreshEntity);

                var loginRes = new PatientLoginResDto
                {
                    Patient_id = patient.Patient_id,
                    Patient_name = patient.Patient_name,
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
