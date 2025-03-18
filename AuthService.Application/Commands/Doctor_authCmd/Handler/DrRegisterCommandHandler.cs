using AuthService.Application.Commands.Doctor_authCmd;
using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Doctor_authCmd.Handler
{
    public class DrRegisterCommandHandler : IRequestHandler<DrRegisterCommand, string>
    {
        private readonly IDrRepo _drRepo;
        private readonly IEmailService _emailService;
        public DrRegisterCommandHandler(IDrRepo drRepo, IEmailService emailService)
        {
            _drRepo = drRepo;
            _emailService = emailService;
        }

        public async Task<string> Handle(DrRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool isExists = await _drRepo.EmailExists(request.Email);
                if (isExists)
                {
                    throw new ValidationException("Email is already taken!");
                }

                await _drRepo.IsVerifyIdentityExists(request.Email);

                Random random = new Random();
                int otp = random.Next(100000, 1000000);

                await _emailService.SendOtp(request.Email, otp);

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

                var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                var expiryTime = currentTime.AddMinutes(5);

                var otpTempStore = new VerifyIdentity
                {
                    Name = request.Doctor_name,
                    Email = request.Email,
                    Password = hashPassword,
                    Otp = otp,
                    Expire_time = expiryTime,
                };

                await _drRepo.AddVeriFyIdentity(otpTempStore);

                return "Registration success!";

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
