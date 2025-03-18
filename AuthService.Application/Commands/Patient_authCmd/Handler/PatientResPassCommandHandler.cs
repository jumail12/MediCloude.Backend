using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Application.Interfaces.IRepos;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.Commands.Patient_authCmd.Handler
{

    public class PatientResPassCommandHandler : IRequestHandler<PatientResPassCommand, string>
    {
        private readonly IPatientRepo _repo;
        public PatientResPassCommandHandler(IPatientRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> Handle(PatientResPassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Allpatients = await _repo.GetAllPatients();
                var patient = Allpatients.FirstOrDefault(a => a.Email == request.Email);

                if (patient == null)
                {
                    throw new ValidationException("User not found");
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.New_password, salt);

                patient.Password = hashPassword;
                patient.Updated_by = patient.Patient_name;
                patient.Updated_on = DateTime.UtcNow;

                await _repo.SaveAsync();
                return "New password created";
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
