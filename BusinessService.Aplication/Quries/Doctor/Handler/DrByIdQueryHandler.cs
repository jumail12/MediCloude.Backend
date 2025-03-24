using BusinessService.Aplication.Common.DTOs.Doctor;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Quries.Doctor.Handler
{
    public class DrByIdQueryHandler : IRequestHandler<DrByIdQuery, DrByIdResDto>
    {
        private readonly IRequestClient<DrByIdReq> _requestClient;
        private readonly ISpecializationRepo _specRepo;
        public DrByIdQueryHandler(IRequestClient<DrByIdReq> requestClient,ISpecializationRepo specializationRepo)
        {
            _requestClient = requestClient;
            _specRepo = specializationRepo;
        }

        public async Task<DrByIdResDto> Handle(DrByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<DrByIdResponse>(new DrByIdReq(request.drid));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var doctor = RabbitMqRes.Message;
                var specialization = await _specRepo.GetSplByIdAsync(doctor.Specialization_id);

                var res = new DrByIdResDto()
                {
                    Id = request.drid,
                    Doctor_name = doctor.Doctor_name,
                    Email = doctor.Email,
                    Qualification = doctor.Qualification,
                    Category = specialization.Category,
                    Phone = doctor.Phone,
                    About = doctor.About,
                    Profile = doctor.Profile,
                    Gender = doctor.Gender.ToString(),
                    Field_experience = doctor.Field_experience,
                };

                return res;

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
