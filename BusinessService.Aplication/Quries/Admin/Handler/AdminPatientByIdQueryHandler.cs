using BusinessService.Aplication.Common.DTOs.AdminDtos;
using BusinessService.Aplication.Interfaces.IRepos;
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
using static Contarcts.Common.CommonContarct;

namespace BusinessService.Aplication.Quries.Admin.Handler
{
    public class AdminPatientByIdQueryHandler : IRequestHandler<AdminPatientByIdQuery, AdminPatientByIdResDto>
    {
        private readonly IRequestClient<PatientByIdRequest> _requestClient;
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IPaymentRepo _paymentRepo;
        public AdminPatientByIdQueryHandler(IRequestClient<PatientByIdRequest> requestClient,IAppoinmentRepo appoinmentRepo, IPaymentRepo paymentRepo)
        {
            _requestClient = requestClient;
            _appoinmentRepo = appoinmentRepo;
            _paymentRepo = paymentRepo;
        }

        public async Task<AdminPatientByIdResDto> Handle(AdminPatientByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMqRes = await _requestClient.GetResponse<PatientByIdResponse>(new PatientByIdRequest(request.id));
                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var patient = RabbitMqRes.Message;

                var appoinmnets = await _appoinmentRepo.GetAppointments_Patient(patient.Patient_id);
                var payments= await _paymentRepo.GetAllPaymentsByPatientId(patient.Patient_id);


                var toatalAppoinmnets = appoinmnets.Count();
                var appoinment_pending = appoinmnets.Where(a => a.Status == AppointmentStatus.Confirmed).Count();
                var appoinmnet_completed = appoinmnets.Where(a => a.Status == AppointmentStatus.Success).Count();
                var totalSpended= payments.Sum(a=>a.DoctorAmount+a.AdminCommissionAmount);
                var adminProfit = payments.Sum(a => a.AdminCommissionAmount);

                var res = new AdminPatientByIdResDto()
                {
                    Patient_id = patient.Patient_id,
                    Patient_name = patient.Patient_name,
                    Email = patient.Email,
                    total_appoinments_taken = toatalAppoinmnets,
                    toatal_appoinments_pending = appoinment_pending,
                    toatal_appoinments_completed=appoinmnet_completed,
                    IsBlocked=patient.IsBlocked,
                    toatal_spended=totalSpended,
                    your_profit=adminProfit,
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
