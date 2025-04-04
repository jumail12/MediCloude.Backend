using BusinessService.Aplication.Common.DTOs.Appoinment;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Doctor;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Quries.Patient.Handler
{
    public class PatientBookedAppoinmentByIdQueryHandler : IRequestHandler<PatientBookedAppoinmentByIdQuery, PatientAppResDto>
    {
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        private readonly IRequestClient<DrByIdReq> _requestClient;
        private readonly ISpecializationRepo _specialRepo;

        public PatientBookedAppoinmentByIdQueryHandler(IAppoinmentRepo appoinmentRepo, IDrAvailabilityRepo drAvailabilityRepo, IRequestClient<DrByIdReq> requestClient, ISpecializationRepo specialRepo)
        {
            _appoinmentRepo = appoinmentRepo;
            _rAvailabilityRepo = drAvailabilityRepo;
            _requestClient = requestClient;
            _specialRepo = specialRepo;
        }
        public async Task<PatientAppResDto> Handle(PatientBookedAppoinmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinment = await _appoinmentRepo.GetBy_APId(request.appId);
                if (appoinment == null)
                {
                    throw new ValidationException("Not found");
                }

                var RabbitMqRes = await _requestClient.GetResponse<DrByIdResponse>(new DrByIdReq(appoinment.DrId));
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var dr = RabbitMqRes.Message;

                if (dr == null)
                {
                    throw new Exception("dr not found");
                }

                var slot = await _rAvailabilityRepo.GetByForAppinment(appoinment.DrAvailabilityId);
                if (slot == null)
                {
                    throw new Exception("slot not found");
                }

                var spl = await _specialRepo.GetSplByIdAsync((Guid)dr.Specialization_id);

                if (spl == null)
                {
                    throw new Exception("spl not found");
                }

                var res = new PatientAppResDto
                {
                    Phone = dr.Phone,
                    AppointmentDate=slot.AppointmentDate,
                    AppointmentTime = slot?.AppointmentTime ?? TimeSpan.Zero,
                    Doctor_name = dr.Doctor_name,
                    Profile = dr.Profile,
                    Email = dr.Email,
                    Field_experience = dr.Field_experience,
                    Gender = dr.Gender,
                    Id=appoinment.Id,
                    Qualification=dr.Qualification,
                    RoomId=appoinment.RoomId,
                    Specialization=spl.Category,
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
