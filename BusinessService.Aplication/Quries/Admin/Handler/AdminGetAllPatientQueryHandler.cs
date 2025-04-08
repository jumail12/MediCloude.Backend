using BusinessService.Aplication.Common.DTOs.AdminDtos;
using BusinessService.Domain.Entities;
using Contarcts.Requests.Patient;
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
    public class AdminGetAllPatientQueryHandler : IRequestHandler<AdminGetAllPatientQuery,AdminGetAllPatient_PaginationResDto>
    {
        private readonly IRequestClient<GetAllPatientReq> _requestClient;
        public AdminGetAllPatientQueryHandler(IRequestClient<GetAllPatientReq> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<AdminGetAllPatient_PaginationResDto> Handle(AdminGetAllPatientQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var RabbitMqRes = await _requestClient.GetResponse<GetAllPatientResponse>(new GetAllPatientReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var AllPatients = RabbitMqRes.Message.patients;

                if (request.name != null)
                {
                    var patients = AllPatients.Where(a=>a.Patient_name.Contains(request.name))
                        .Select(b=> new GetAllPatient_ResDto
                        {
                            Email = b.Email,
                            Patient_name = b.Patient_name,
                            Patient_id = b.Patient_id,
                            IsBlocked = b.IsBlocked,
                        })
                         .Skip((request.pageNumber - 1) * request.pageSize)
                        .Take(request.pageSize)
                        .ToList();

                    var resQ = new AdminGetAllPatient_PaginationResDto
                    {
                        total_pages=(int)Math.Ceiling((double)AllPatients.Count / request.pageSize),
                        items=patients
                    };

                    return resQ;
                }

                var patientsRes = AllPatients
                   .Select(b => new GetAllPatient_ResDto
                   {
                       Email = b.Email,
                       Patient_name = b.Patient_name,
                       Patient_id = b.Patient_id,
                       IsBlocked = b.IsBlocked,
                   })
                    .Skip((request.pageNumber - 1) * request.pageSize)
                   .Take(request.pageSize)
                   .ToList();

                var res = new AdminGetAllPatient_PaginationResDto
                {
                    total_pages = (int)Math.Ceiling((double)AllPatients.Count / request.pageSize),
                    items = patientsRes
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
