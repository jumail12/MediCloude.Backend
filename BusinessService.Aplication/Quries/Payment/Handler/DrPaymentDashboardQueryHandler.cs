using BusinessService.Aplication.Common.DTOs.Payment;
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
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
using static Contarcts.Common.CommonContarct;

namespace BusinessService.Aplication.Quries.Payment.Handler
{
    public class DrPaymentDashboardQueryHandler : IRequestHandler<DrPaymentDashboardQuery, DrPaymentDashboardResDto>
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IRequestClient<GetAllPatientReq> _requestClient;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        public DrPaymentDashboardQueryHandler(IPaymentRepo paymentRepo, IAppoinmentRepo appoinmentRepo, IDrAvailabilityRepo rAvailabilityRepo,IRequestClient<GetAllPatientReq> requestClient)
        {
            _paymentRepo = paymentRepo;
            _appoinmentRepo = appoinmentRepo;
            _rAvailabilityRepo = rAvailabilityRepo;
            _requestClient = requestClient;
        }

        public async Task<DrPaymentDashboardResDto> Handle(DrPaymentDashboardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var payments = await _paymentRepo.GetAllPaymentDetailsByDrId(request.drid);

                var appoinmnets= await _appoinmentRepo.GetAppoinmentByDrId(request.drid);
                var appoDict = appoinmnets.ToDictionary(a => a.Id, a => a);
                if (appoinmnets == null)
                {
                    throw new Exception("Appoinments not found");
                }

                var slots = await _rAvailabilityRepo.GetAllSlots();
                var slotDict = slots.ToDictionary(s => s.Id, s => s);

                var RabbitMqRes = await _requestClient.GetResponse<GetAllPatientResponse>(new GetAllPatientReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var AllPatients = RabbitMqRes.Message.patients;
                var AllPatientDict = AllPatients.ToDictionary(a => a.Patient_id, a => a);


                var profitDr = payments.Where(a=>a.DoctorId==request.drid && a.PaymentStatus.ToString()== "Success").Sum(a => a.DoctorAmount);
                var paymentPending = payments.Where(a => a.PaymentStatus == Status.Pending).Count();
                var paymentFailed= payments.Where(a => a.PaymentStatus == Status.Failed).Count();
                var toatal_appoinmnets = appoinmnets.Count();
                var appoinment_pending = appoinmnets.Where(a => a.Status == AppointmentStatus.Confirmed).Count();
                var appoinmnet_completed = appoinmnets.Where(a => a.Status==AppointmentStatus.Success).Count();


                var paymentDetails = new DrPaymentDetails_paginationDto
                {
                  total_pages = (int)Math.Ceiling((double)payments.Count / request.pageSize),
                  drPayments=payments.Select(a=> new DrPaymentDetailsResDto
                  {
                      Id = a.Id,
                      AppointmentDate=appoDict.TryGetValue(a.AppointmentId, out var appo) &&
                                    slotDict.TryGetValue(appo.DrAvailabilityId, out var slot)
                                        && slot.AppointmentDate != null
                                          ? slot.AppointmentDate
                                          : DateTime.MinValue,
                      AppointmentTime = appoDict.TryGetValue(a.AppointmentId, out var appoTime) &&
                                   slotDict.TryGetValue(appoTime.DrAvailabilityId, out var slottime)
                                   ? slottime.AppointmentTime
                                   : TimeSpan.Zero,
                      DoctorAmount=a.DoctorAmount,
                      Email=AllPatientDict.TryGetValue(appo.PatientId, out var patient) ? patient.Email : null,
                      Patient_name=patient.Patient_name,
                      TransactionId=a.TransactionId,
                      PaymentMethod=a.PaymentMethod.ToString(),
                      PaymentStatus=a.PaymentStatus.ToString(),
                  })
                     .Skip((request.pageNumber - 1) * request.pageSize)
                     .Take(request.pageSize)
                     .ToList(),
                };

                var res = new DrPaymentDashboardResDto
                {
                    profit=profitDr,
                    total_appoinments_taken=toatal_appoinmnets,
                    toatal_appoinments_pending=appoinment_pending,
                    toatal_appoinments_completed=appoinmnet_completed,
                    payment_pending=paymentPending,
                    payment_failed=paymentFailed,
                    payment_deatils=paymentDetails
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
