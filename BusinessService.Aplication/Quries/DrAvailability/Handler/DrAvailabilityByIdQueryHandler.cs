using BusinessService.Aplication.Common.DTOs.Availability;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.DrAvailability.Handler
{
    public class DrAvailabilityByIdQueryHandler : IRequestHandler<DrAvailabilityByIdQuery, List<DrAvailabilityByIdResDto>>
    {
        private readonly IDrAvailabilityRepo _repo;
        private readonly IRequestClient<DrByIdReq> _requestClient;
        public DrAvailabilityByIdQueryHandler(IDrAvailabilityRepo repo,IRequestClient<DrByIdReq> requestClient)
        {
            _requestClient = requestClient;
            _repo = repo;
        }

        public async Task<List<DrAvailabilityByIdResDto>> Handle(DrAvailabilityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var RabbitMqRes = await _requestClient.GetResponse<DrByIdResponse>(new DrByIdReq(request.drid));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var doctor = RabbitMqRes.Message;

                var slots= await _repo.GetByDrId(doctor.Id);

                var res= slots.
                    GroupBy(a=>a.AppointmentDay)
                    .Select(grp=> new DrAvailabilityByIdResDto
                    {
                        AppointmentDay=grp.Key.ToString(),
                        AppoinmentTimes=grp.Select(b=> new DrAvailabiliyTimeSlotDto
                        {
                            Id=b.Id,
                            AppointmentTime= DateTime.Today.Add(b.AppointmentTime).ToString("hh:mm tt"),
                            IsAvailable=b.IsAvailable,
                        }).ToList()
                    }).ToList();

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
