using BusinessService.Aplication.Common.DTOs.AdminDtos;
using BusinessService.Aplication.Interfaces.IRepos;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;
using static Contarcts.Common.CommonContarct;

namespace BusinessService.Aplication.Quries.Admin
{
    public class AdminPaymentDashBoardQueryHandler : IRequestHandler<AdminPaymentDashBoardQuery, AdminPaymentDashBoardResDto>
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IRequestClient<GetAllPatientReq> _requestClient;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;

        public AdminPaymentDashBoardQueryHandler(IPaymentRepo paymentRepo, IAppoinmentRepo appoinmentRepo, IRequestClient<GetAllPatientReq> requestClient, IDrAvailabilityRepo rAvailabilityRepo)
        {
            _paymentRepo = paymentRepo;
            _appoinmentRepo = appoinmentRepo;
            _requestClient = requestClient;
            _rAvailabilityRepo = rAvailabilityRepo;
        }

        public async Task<AdminPaymentDashBoardResDto> Handle(AdminPaymentDashBoardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allPayments = await _paymentRepo.GetALlPayments();

                var allAppoinmnets = await _appoinmentRepo.GetAll();
                var appoDict = allAppoinmnets.ToDictionary(a => a.Id, a => a);

                var slots = await _rAvailabilityRepo.GetAllSlots();
                var slotDict = slots.ToDictionary(s => s.Id, s => s);

                var RabbitMqRes = await _requestClient.GetResponse<GetAllPatientResponse>(new GetAllPatientReq());
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }
                var AllPatients = RabbitMqRes.Message.patients;
                var AllPatientDict = AllPatients.ToDictionary(a => a.Patient_id, a => a);

                var tottalSales = allPayments.Where(b => b.PaymentStatus == Status.Success).Sum(c => c.AdminCommissionAmount + c.DoctorAmount);
                var profit_ = allPayments.Where(a=> a.PaymentStatus== Status.Success).Sum(a => a.AdminCommissionAmount);
                var paymentPending = allPayments.Where(a => a.PaymentStatus == Status.Pending).Count();
                var paymentFailed = allPayments.Where(a => a.PaymentStatus == Status.Failed).Count();
                var toatal_appoinmnets = allAppoinmnets.Count();
                var appoinment_pending = allAppoinmnets.Where(a => a.Status == AppointmentStatus.Confirmed).Count();
                var appoinmnet_completed = allAppoinmnets.Where(a => a.Status == AppointmentStatus.Success).Count();

                var paymentDetails = new AdminPaymentDetails_paginationDto
                {
                    total_pages = (int)Math.Ceiling((double)allPayments.Count / request.pageSize),
                    items = allPayments.Select(a => new AdminPaymentDetailsResDto
                    {
                        Id = a.Id,
                        AppointmentDate = appoDict.TryGetValue(a.AppointmentId, out var appo) &&
                                        slotDict.TryGetValue(appo.DrAvailabilityId, out var slot) && slot.AppointmentDate != null
                                           ? slot.AppointmentDate
                                           : DateTime.MinValue,
                        AppointmentTime = appoDict.TryGetValue(a.AppointmentId, out var appoTime) &&
                                    slotDict.TryGetValue(appoTime.DrAvailabilityId, out var slottime)
                                    ? slottime.AppointmentTime
                                    : TimeSpan.Zero,
                        Amount = a.DoctorAmount + a.AdminCommissionAmount,
                        Email = AllPatientDict.TryGetValue(a.PatientId, out var patient) ? patient.Email : null,
                        Patient_name = patient.Patient_name,
                        PaymentMethod = a.PaymentMethod.ToString(),
                        PaymentStatus = a.PaymentStatus.ToString(),
                        TransactionId = a.TransactionId,
                    })
                           .Skip((request.pageNumber - 1) * request.pageSize)
                     .Take(request.pageSize)
                   .ToList(),
                };

                var res = new AdminPaymentDashBoardResDto
                {
                    sales=tottalSales,
                    payment_failed=paymentFailed,
                    payment_pending=paymentPending,
                    profit=profit_,
                    toatal_appoinments_pending=appoinment_pending,
                    total_appoinments_taken=toatal_appoinmnets,
                    toatal_appoinments_completed = appoinmnet_completed,
                    payment_deatils =paymentDetails,
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
