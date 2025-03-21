using AuthService.Application.Interfaces.IRepos;
using Contarcts.Requests.Specialization;
using Contarcts.Responses.Specialization;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.Commands.Doctor_authCmd.Handler
{
    public class DrLicenseVerifyCommandHandler : IRequestHandler<DrLicenseVerifyCommand, string>
    {
        private readonly IRequestClient<SpecializationExistsReq> _requestClient;
        private readonly IDrRepo _repo;

        public DrLicenseVerifyCommandHandler(IRequestClient<SpecializationExistsReq> requestClient,IDrRepo drRepo)
        {
            _repo = drRepo;
            _requestClient = requestClient;
        }

        public async Task<string> Handle(DrLicenseVerifyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _requestClient.GetResponse<SpecializationExistsResponse>(new SpecializationExistsReq(Guid.Parse(request.Specialization_id)));
                var isEx = res.Message;
                if (res == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                if (!isEx.Exists)
                {
                    throw new Exception("Specialization category not found");
                }


                var allDrs= await _repo.GetAllDrs();
                var doctor= allDrs.FirstOrDefault(a=>a.Email == request.Email);

                if (doctor == null)
                {
                    throw new ValidationException("dr not found");
                }

                doctor.Specialization_id=Guid.Parse(request.Specialization_id);
                doctor.Medical_license_number=request.Medical_license_number;
                doctor.Updated_by = doctor.Doctor_name;
                doctor.Updated_on = DateTime.UtcNow;

                await _repo.SaveAsync();

                return "Your details are validate and approve later";
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
