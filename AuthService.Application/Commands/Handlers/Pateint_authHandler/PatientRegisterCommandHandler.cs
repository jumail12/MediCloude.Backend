
using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Handlers.Pateint
{
    public class PatientRegisterCommandHandler : IRequestHandler<PatientRegisterCommand, string>
    {
        private readonly IEmailService _emailService;
        private readonly IPatientRepo _patientRepo;
        public PatientRegisterCommandHandler(IPatientRepo repo, IEmailService emailService)
        {
            _emailService = emailService;
            _patientRepo = repo;
        }

        public async Task<string> Handle(PatientRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isExist = await _patientRepo.EmailExists(request.Email);
                if (isExist)
                {
                    throw new ValidationException("Email is already taken!");
                }

                await _patientRepo.IsVerifyIdentityExists(request.Email);

                Random random = new Random();
                int otp = random.Next(100000, 1000000);

                await _emailService.SendOtp(request.Email,otp);

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

                var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                var expiryTime = currentTime.AddMinutes(5);

                var otpTempStore = new VerifyIdentity
                {
                    Name = request.Patient_name,
                    Email = request.Email,
                    Password = hashPassword,
                    Otp = otp,
                    Expire_time = expiryTime,
                };

                await _patientRepo.AddVeriFyIdentity(otpTempStore);

                return "Patient registration success!";
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
    }
}
