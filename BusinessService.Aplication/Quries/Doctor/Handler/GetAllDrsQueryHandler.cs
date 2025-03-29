using AuthService.Domain.Entities;
using BusinessService.Aplication.Common.DTOs.Doctor;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MassTransit.Serialization;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Quries.Doctor.Handler
{
    public class GetAllDrsQueryHandler : IRequestHandler<GetAllDrsQuery, GetAllDrsPageResDto>
    {
        private readonly IRequestClient<GetAllDrsReq> _requestClient;
        private readonly ISpecializationRepo _specRepo;
        public GetAllDrsQueryHandler(IRequestClient<GetAllDrsReq> requestClient,ISpecializationRepo specializationRepo)
        {
            _requestClient = requestClient;
            _specRepo = specializationRepo;
        }

        public async Task<GetAllDrsPageResDto> Handle(GetAllDrsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<GetAllDrsResponse>(new GetAllDrsReq());

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var Alldoctors = RabbitMqRes.Message.doctors;

                var specializations = await _specRepo.GetAllSplAsync();
                var specializationDict = specializations.ToDictionary(s => s.Id, s => s.Category);

                var drs = Alldoctors.Select(x => new GetAllDrsResDto
                {
                    Id = x.Id,
                    Doctor_name = x.Doctor_name,
                    Qualification = x.Qualification,
                    Category = specializationDict.TryGetValue((Guid)x.Specialization_id, out var category) ? category : "Unknown",
                    Profile = x.Profile,
                    Field_experience = x.Field_experience,
                    Drfee = x.Drfee,
                })
                     .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToList();

                var res = new GetAllDrsPageResDto
                {
                    total_pages= (int)Math.Ceiling((double)Alldoctors.Count / request.pageSize),
                    doctors = drs,
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
