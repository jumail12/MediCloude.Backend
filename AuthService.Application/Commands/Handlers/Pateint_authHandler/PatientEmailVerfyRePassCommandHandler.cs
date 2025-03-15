using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Handlers.Pateint_authHandler
{
    public class PatientEmailVerfyRePassCommandHandler : IRequestHandler<PatientEmailVerfyRePassCommand, string>
    {
        private readonly IPatientRepo _repo;
        public PatientEmailVerfyRePassCommandHandler(IPatientRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> Handle(PatientEmailVerfyRePassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var verifiedUsers = await _repo.GetAllVeriFyIdentity();
                var veriUser = verifiedUsers.FirstOrDefault(a => a.Email == request.email && a.Otp == request.otp);

                if (veriUser == null)
                {
                    throw new ValidationException("User not found");
                }

                TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
                if (currentTime <= veriUser.Expire_time)
                {
                    await _repo.RemoveVeriFyIdentity(veriUser);
                    return "Otp verification completed!";
                }

                return "Your otp is expired!";
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
