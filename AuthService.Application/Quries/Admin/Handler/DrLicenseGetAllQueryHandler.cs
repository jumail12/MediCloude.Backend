using AuthService.Application.Common.DTOs.AdminDTOs;
using AuthService.Application.Interfaces.IRepos;
using Contarcts.Requests.Specialization;
using Contarcts.Responses.Specialization;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.Quries.Admin.Handler
{
    public class DrLicenseGetAllQueryHandler : IRequestHandler<DrLicenseGetAllQuery,DrLicenseGetAllPageResDto>
    {
        private readonly IDrRepo _drRepo;
        private readonly IRequestClient<GetAllSpecializationReq> _requestClient;
        public DrLicenseGetAllQueryHandler(IDrRepo drRepo,IRequestClient<GetAllSpecializationReq> requestClient)
        {
            _drRepo = drRepo;
            _requestClient = requestClient;
        }

        public async Task<DrLicenseGetAllPageResDto> Handle(DrLicenseGetAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var drs = await _drRepo.GetAllLicenseVeriDrs();
                var RabbitMqRes = await _requestClient.GetResponse<GetAllSpecializationResponseList>(new GetAllSpecializationReq());

                var allSpecializations = RabbitMqRes.Message.specializations;

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var res = new DrLicenseGetAllPageResDto()
                {
                    total_pages = (int)Math.Ceiling((double)drs.Count / request.pageSize),
                    items = drs
                    .OrderByDescending(n => n.Created_on)
                    .Select(a => new DrLicenseGetAllResDto
                    {
                        Id = a.Id,
                        Doctor_name = a.Doctor_name,
                        Email = a.Email,
                        Medical_license_number = a.Medical_license_number,
                        Specialization = allSpecializations
                                     .Where(b => b.splId == a.Specialization_id)
                                     .Select(c => c.Category)
                                     .FirstOrDefault() ?? "Unknown",
                    })
                       .Skip((request.pageNumber - 1) * request.pageSize)
                     .Take(request.pageSize)
                    .ToList(),
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
