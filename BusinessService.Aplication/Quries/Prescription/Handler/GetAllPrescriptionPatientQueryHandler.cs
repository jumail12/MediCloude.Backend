using BusinessService.Aplication.Common.DTOs.Prescription;
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
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

namespace BusinessService.Aplication.Quries.Prescription.Handler
{
    public class GetAllPrescriptionPatientQueryHandler : IRequestHandler<GetAllPrescriptionPatientQuery, PatientPrescription_PaginationResDto>
    {
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        private readonly IRequestClient<GetAllDrsReq> _requestClient;
        private readonly ISpecializationRepo _SpecializationRepo;
        private readonly IPrescriptionRepo _prescriptionRepo;
        public GetAllPrescriptionPatientQueryHandler(IAppoinmentRepo appoinmentRepo,IDrAvailabilityRepo drAvailabilityRepo,IRequestClient<GetAllDrsReq> requestClient,ISpecializationRepo specializationRepo,IPrescriptionRepo prescriptionRepo)
        {
            _appoinmentRepo = appoinmentRepo;
            _rAvailabilityRepo = drAvailabilityRepo;
            _requestClient = requestClient;
            _SpecializationRepo = specializationRepo;
            _prescriptionRepo = prescriptionRepo;
        }


        public async Task<PatientPrescription_PaginationResDto> Handle(GetAllPrescriptionPatientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinmnets = await _appoinmentRepo.GetByPatientID_Prescription(request.patientid);
                var appoDict= appoinmnets.ToDictionary(a=>a.Id,a=>a );
                if (appoinmnets == null)
                {
                    throw new Exception("Appoinments not found");
                }

                var RabbitMqRes = await _requestClient.GetResponse<GetAllDrsResponse>(new GetAllDrsReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var AllDrs = RabbitMqRes.Message.doctors;
                var AllDrDict = AllDrs.ToDictionary(a => a.Id, a => a);

                var slots = await _rAvailabilityRepo.GetAllSlots();
                var slotDict = slots.ToDictionary(s => s.Id, s => s);

                var specilalizations = await _SpecializationRepo.GetAllSplAsync();
                var splDict = specilalizations.ToDictionary(a => a.Id, a => a);

                var AllPrescriptions = await _prescriptionRepo.GetPrescriptionList_Patient(request.patientid);

                if (AllPrescriptions == null)
                {
                    throw new Exception("Prescriptions not found");
                }

                var res = AllPrescriptions
                      .OrderByDescending(n => n.Created_on)
                    .Select(x => new PatientPrescriptionResDto
                {
                    Id = x.Id,
                    PrescriptionText = x.PrescriptionText,
                    Doctor_name = AllDrDict.TryGetValue(x.DrId, out var dr) ? dr.Doctor_name : "Unknown Doctor",
                    AppointmentDate = appoDict.TryGetValue(x.AppointmentID, out var appo) &&
                                    slotDict.TryGetValue(appo.DrAvailabilityId, out var slot)
                                        && slot.AppointmentDate != null
                                          ? slot.AppointmentDate
                                          : DateTime.MinValue,
                    AppointmentTime = appoDict.TryGetValue(x.AppointmentID, out var appoTime) &&
                  slotDict.TryGetValue(appoTime.DrAvailabilityId, out var slottime)
                      ? slottime.AppointmentTime
                      : TimeSpan.Zero,
                    Category=splDict.TryGetValue((Guid)dr.Specialization_id, out var spl)? spl.Category : null,
                    Email=dr.Email,
                    Field_experience=dr.Field_experience,
                    Phone=dr.Phone,
                    Profile=dr.Profile,
                    Qualification=dr.Qualification,
                })
                     .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToList();

                return new PatientPrescription_PaginationResDto
                {
                    items=res,
                    total_pages = (int)Math.Ceiling((double)appoinmnets.Count / request.pageSize),
                };
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
