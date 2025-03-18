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
    public class DrForgotPasswordCommandHandler : IRequestHandler<DrForgotPasswordCommand, string>
    {
        private readonly IDrRepo _repo;
        private readonly IEmailService _emailService;
        public DrForgotPasswordCommandHandler(IDrRepo drRepo, IEmailService emailService)
        {
            _repo = drRepo;
            _emailService = emailService;
        }

        public async Task<string> Handle(DrForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allDrs= await _repo.GetAllDrs();
                var dr= allDrs.FirstOrDefault(a=>a.Email==request.Email);

                if (dr == null)
                {
                    throw new ValidationException("Not found");
                }

                await _repo.IsVerifyIdentityExists(request.Email);
                Random random = new Random();
                int otp = random.Next(100000, 1000000);

                await _emailService.SendOtp(request.Email, otp);

                var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                var expiryTime = currentTime.AddMinutes(5);

                var otpTempStore = new VerifyIdentity
                {
                    Name = dr.Doctor_name,
                    Email = dr.Email,
                    Password = dr.Password,
                    Otp = otp,
                    Expire_time = expiryTime,
                };

                await _repo.AddVeriFyIdentity(otpTempStore);

                return "Check your mail to get verfication Otp";

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
