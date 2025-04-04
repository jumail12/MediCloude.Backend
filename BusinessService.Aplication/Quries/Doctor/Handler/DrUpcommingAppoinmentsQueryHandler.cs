using BusinessService.Aplication.Common.DTOs.Appoinment;
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Doctor.Handler
{
    public class DrUpcommingAppoinmentsQueryHandler : IRequestHandler<DrUpcommingAppoinmentsQuery, AppoinmentPaginationResDto>
    {
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        private readonly IRequestClient<GetAllPatientReq> _requestClient;
        public DrUpcommingAppoinmentsQueryHandler(IAppoinmentRepo appoinmentRepo, IDrAvailabilityRepo rAvailabilityRepo,IRequestClient<GetAllPatientReq> requestClient)
        {
            _appoinmentRepo = appoinmentRepo;
            _rAvailabilityRepo = rAvailabilityRepo;
            _requestClient = requestClient;
        }

        public async Task<AppoinmentPaginationResDto> Handle(DrUpcommingAppoinmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinments = await _appoinmentRepo.GetAppoinmentListByDrId(request.id);

                var RabbitMqRes = await _requestClient.GetResponse<GetAllPatientResponse>(new GetAllPatientReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var AllPatients = RabbitMqRes.Message.patients;
                var AllPatientDict = AllPatients.ToDictionary(a => a.Patient_id, a => a);

                var slots= await _rAvailabilityRepo.GetAllSlots();
                var drslots= slots.Where(a=>a.DrId == request.id).ToList();  
                var drSlotsDict=drslots.ToDictionary(a=>a.Id, a => a);

                var resAppo = appoinments
                    .OrderByDescending(x => x.Created_on) 
                    .Select(x => new DrUpcommingAppoinmentsResDto
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        Email = AllPatientDict.TryGetValue(x.PatientId, out var patient) ? patient.Email : null,
                        Patient_name = patient?.Patient_name, 
                        AppointmentDate = drSlotsDict.TryGetValue(x.DrAvailabilityId, out var slot) && slot.AppointmentDate != null
                                          ? slot.AppointmentDate
                                          : DateTime.MinValue,
                        AppointmentTime = slot?.AppointmentTime ?? TimeSpan.Zero, 
                    })
                    .Skip((request.pageNumber - 1) * request.pageSize) 
                    .Take(request.pageSize)
                    .ToList();


                var res = new AppoinmentPaginationResDto
                {
                   total_pages = (int)Math.Ceiling((double)appoinments.Count / request.pageSize),
                   items=resAppo
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
