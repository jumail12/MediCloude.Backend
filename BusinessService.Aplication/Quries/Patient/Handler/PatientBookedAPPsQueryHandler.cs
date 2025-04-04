using BusinessService.Aplication.Common.DTOs.Appoinment;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Quries.Patient.Handler
{
    public class PatientBookedAPPsQueryHandler : IRequestHandler<PatientBookedAPPsQuery, AppoinmentPatientPaginationResDto>
    {
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        private readonly IRequestClient<GetAllDrsReq> _requestClient;
        private readonly ISpecializationRepo _SpecializationRepo;
        public PatientBookedAPPsQueryHandler(IAppoinmentRepo appoinmentRepo, IDrAvailabilityRepo rAvailabilityRepo, IRequestClient<GetAllDrsReq> requestClient, ISpecializationRepo specializationRepo)
        {
            _appoinmentRepo = appoinmentRepo;
            _rAvailabilityRepo = rAvailabilityRepo;
            _requestClient = requestClient;
            _SpecializationRepo = specializationRepo;
        }
        public async Task<AppoinmentPatientPaginationResDto> Handle(PatientBookedAPPsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinments = await _appoinmentRepo.GetByPatientID(request.PatientId);

                var RabbitMqRes = await _requestClient.GetResponse<GetAllDrsResponse>(new GetAllDrsReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var AllDrs = RabbitMqRes.Message.doctors;
                var AllDrDict = AllDrs.ToDictionary(a => a.Id, a => a);

                var slots = await _rAvailabilityRepo.GetAllSlots();
                var slotDict= slots.ToDictionary(s=> s.Id, s => s);

                var specilalizations = await _SpecializationRepo.GetAllSplAsync();
                var splDict= specilalizations.ToDictionary(a=>a.Id, a => a);
 

                var patientAppo = appoinments
                    .OrderByDescending(x=>x.Created_on)
                    .Select(a=> new PatientAppResDto
                {
                    Id = a.Id,
                    RoomId = a.RoomId,
                    Doctor_name=AllDrDict.TryGetValue(a.DrId, out var dr) ? dr.Doctor_name : null,
                    Email=dr.Email,
                    Field_experience=dr.Field_experience,
                    Gender=dr.Gender.ToString(),
                    Phone=dr.Phone,
                    Profile=dr.Profile,
                    Qualification=dr.Qualification,
                    AppointmentDate=slotDict.TryGetValue(a.DrAvailabilityId, out var slot) && slot.AppointmentDate != null
                                          ? slot.AppointmentDate
                                          : DateTime.MinValue,
                    AppointmentTime= slot?.AppointmentTime ?? TimeSpan.Zero,
                    Specialization=splDict.TryGetValue((Guid)dr.Specialization_id, out  var sp)? sp.Category.ToString() : null ,
                      
                    })
                          .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToList();


                var res = new AppoinmentPatientPaginationResDto
                {
                    total_pages= (int)Math.Ceiling((double)appoinments.Count / request.pageSize),
                    items = patientAppo
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
