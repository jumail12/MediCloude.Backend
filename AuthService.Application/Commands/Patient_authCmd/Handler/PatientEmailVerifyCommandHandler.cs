using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Interfaces.IRepos;
using AuthService.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.Commands.Patient_authCmd.Handler
{
    public class PatientEmailVerifyCommandHandler : IRequestHandler<PatientEmailVerifyCommand, string>
    {
        private readonly IPatientRepo _repo;
        public PatientEmailVerifyCommandHandler(IPatientRepo repo)
        {
            _repo = repo;
        }

        public async Task<string> Handle(PatientEmailVerifyCommand request, CancellationToken cancellationToken)
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
                    var patient_ = new Patient
                    {
                        Patient_name = veriUser.Name,
                        Email = veriUser.Email,
                        Password = veriUser.Password,
                        Created_by = veriUser.Name,
                        Created_on = DateTime.UtcNow,
                        Updated_by = veriUser.Name,
                        Updated_on = DateTime.UtcNow
                    };

                    await _repo.AddNewVerifiedPatient(patient_);
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
