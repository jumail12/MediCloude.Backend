using BusinessService.Aplication.Common.DTOs.Appoinment;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Quries.Doctor.Handler
{
    public class DrUPAPByIdQueryHandler : IRequestHandler<DrUPAPByIdQuery, DrUpcommingAppoinmentsResDto>
    {
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        private readonly IRequestClient<PatientByIdRequest> _requestClient;
        public DrUPAPByIdQueryHandler(IAppoinmentRepo appoinmentRepo,IDrAvailabilityRepo drAvailabilityRepo,IRequestClient<PatientByIdRequest> requestClient)
        {
            _appoinmentRepo = appoinmentRepo;
            _rAvailabilityRepo = drAvailabilityRepo;
            _requestClient=requestClient;
        }
        public async Task<DrUpcommingAppoinmentsResDto> Handle(DrUPAPByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinment= await _appoinmentRepo.GetBy_APId(request.apId);
                if (appoinment == null)
                {
                    throw new ValidationException("Not found");
                }

                var RabbitMqRes = await _requestClient.GetResponse<PatientByIdResponse>(new PatientByIdRequest(appoinment.PatientId));
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var patient = RabbitMqRes.Message;

                if (patient == null)
                {
                    throw new Exception("Patient not found");
                }

                var slot= await _rAvailabilityRepo.GetByForAppinment(appoinment.DrAvailabilityId);

                var res = new DrUpcommingAppoinmentsResDto
                {
                    PatientId=patient.Patient_id,
                    Patient_name=patient.Patient_name,
                    Email=patient.Email,
                    Id=appoinment.Id,
                    AppointmentDate=slot.AppointmentDate,
                    AppointmentTime = slot?.AppointmentTime ?? TimeSpan.Zero,
                    RoomId=appoinment.RoomId,
                    AppoinmentStatus=appoinment.Status.ToString(),
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
