using BusinessService.Aplication.Common.DTOs.AdminDtos;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Doctor;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Admin.Handler
{
    public class AdminGetAllDrQueryHandler : IRequestHandler<AdminGetAllDrQuery, AdminGetAllDoctor_PaginationResDto>
    {
        private readonly IRequestClient<GetAllDrsReq> _requestClient;
        private readonly ISpecializationRepo _SpecializationRepo;
        public AdminGetAllDrQueryHandler(IRequestClient<GetAllDrsReq> requestClient,ISpecializationRepo specializationRepo)
        {
            _requestClient = requestClient;
            _SpecializationRepo = specializationRepo;
        }

        public async Task<AdminGetAllDoctor_PaginationResDto> Handle(AdminGetAllDrQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<GetAllDrsResponse>(new GetAllDrsReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var AllDrs = RabbitMqRes.Message.doctors;

                var specialization = await _SpecializationRepo.GetAllSplAsync();
                var splDict=specialization.ToDictionary(b=>b.Id, b => b);

                if (request.name != null)
                {
                    var resDrq = AllDrs.Where(m=>m.Doctor_name.Contains(request.name)).Select(a => new GetAllDoctor_ResDto
                    {
                        Doctor_name = a.Doctor_name,
                        Id = a.Id,
                        Field_experience = a.Field_experience,
                        IsBlocked = a.IsBlocked,
                        Profile = a.Profile,
                        Qualification = a.Qualification,
                        Specialization = splDict.TryGetValue((Guid)a.Specialization_id, out var spl) ? spl.Category.ToString() : null,
                    })
                  .Skip((request.pageNumber - 1) * request.pageSize)
                 .Take(request.pageSize)
                .ToList();

                    var resQ = new AdminGetAllDoctor_PaginationResDto
                    {
                        total_pages = (int)Math.Ceiling((double)AllDrs.Count / request.pageSize),
                        items = resDrq,
                    };
                    return resQ;
                }

                var resdrs = AllDrs.Select(a => new GetAllDoctor_ResDto
                {
                    Doctor_name = a.Doctor_name,
                    Id = a.Id,
                    Field_experience = a.Field_experience,
                    IsBlocked = a.IsBlocked,
                    Profile = a.Profile,
                    Qualification = a.Qualification,
                    Specialization = splDict.TryGetValue((Guid)a.Specialization_id, out var spl)? spl.Category.ToString() : null,
                })
                      .Skip((request.pageNumber - 1) * request.pageSize)
                     .Take(request.pageSize)
                    .ToList();

                var res = new AdminGetAllDoctor_PaginationResDto
                {
                   total_pages = (int)Math.Ceiling((double)AllDrs.Count / request.pageSize),
                    items = resdrs,
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
