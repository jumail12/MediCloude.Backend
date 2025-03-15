using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.Commands.Handlers.Pateint_authHandler
{
    public class PatientForgotPasswordCommandHandler : IRequestHandler<PatientForgotPasswordCommand, string>
    {
      
        private readonly IEmailService _emailService;
        private readonly IPatientRepo _pateintRepo;
        public PatientForgotPasswordCommandHandler(IPatientRepo repo, IEmailService emailService)
        {
            _emailService = emailService;
            _pateintRepo = repo;
        }

        public async Task<string> Handle(PatientForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var allPatients = await _pateintRepo.GetAllPatients();
                var patienT = allPatients.FirstOrDefault(a => a.Email == request.Email);

                if (patienT == null)
                {
                    throw new ValidationException("Patient not found");
                }

                await _pateintRepo.IsVerifyIdentityExists(request.Email);

                Random random = new Random();
                int otp = random.Next(100000, 1000000);

                await _emailService.SendOtp(request.Email, otp);

                var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                var expiryTime = currentTime.AddMinutes(5);

                var otpTempStore = new VerifyIdentity
                {
                    Name = patienT.Patient_name,
                    Email = patienT.Email,
                    Password = patienT.Password,
                    Otp = otp,
                    Expire_time = expiryTime,
                };

                await _pateintRepo.AddVeriFyIdentity(otpTempStore);

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
