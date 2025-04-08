using AuthService.Domain.Entities;
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Aplication.Interfaces.IServices;
using BusinessService.Domain.Entities;
using Contarcts.Common.DTOs;
using Contarcts.Requests.Doctor;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Doctor;
using Contarcts.Responses.Patient;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;
using static Contarcts.Common.CommonContarct;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace BusinessService.Aplication.Commands.Prescription.Handler
{
    public class PrescriptionAddCommandHandler : IRequestHandler<PrescriptionAddCommand, string>
    {
        private readonly IPrescriptionRepo _repo;
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IRequestClient<PatientByIdRequest> _requestClient;
        private readonly IDrAvailabilityRepo _rvailabilityRepo;
        private readonly INotificationService _notificationService;
        private readonly IRequestClient<DrByIdReq> _reqClient1;
        public PrescriptionAddCommandHandler(IPrescriptionRepo prescriptionRepo, IAppoinmentRepo appoinmentRepo,IRequestClient<PatientByIdRequest> requestClient,IDrAvailabilityRepo drAvailabilityRepo,INotificationService notification,IRequestClient<DrByIdReq> requestClient1)
        {
            _repo = prescriptionRepo;
            _appoinmentRepo = appoinmentRepo;
            _requestClient = requestClient;
            _rvailabilityRepo = drAvailabilityRepo;
            _notificationService = notification;
            _reqClient1 = requestClient1;
        }
        public async Task<string> Handle(PrescriptionAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinment = await _appoinmentRepo.GetBy_APId(request.AppId);
                if (appoinment == null)
                {
                    throw new Exception("Appoinmnet is not found");
                }

                var RabbitMqRes = await _requestClient.GetResponse<PatientByIdResponse>(new PatientByIdRequest(appoinment.PatientId));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var patient = RabbitMqRes.Message;

                if (patient == null)
                {
                    throw new Exception("patient not found");
                }

                var RabbittDr= await _reqClient1.GetResponse<DrByIdResponse>(new DrByIdReq(appoinment.DrId));
                if (RabbittDr == null)
                {
                    throw new Exception("Error making communication with rabbitMq");
                }

                var doctor = RabbittDr.Message;
                if (doctor == null)
                {
                    throw new Exception("dr not found");
                }

                var slot = await _rvailabilityRepo.GetById(appoinment.DrAvailabilityId);

                if (slot == null)
                {
                    throw new Exception("slot not found");
                }

                appoinment.Status = AppointmentStatus.Success;
                await _appoinmentRepo.SaveAsync();

                var newPres = new Domain.Entities.Prescription
                {
                   AppointmentID= appoinment.Id,
                   PrescriptionText=request.PrescriptionText,
                   DrId= appoinment.DrId,
                   PatientId= appoinment.PatientId,
                };
                
                await _repo.AddPrescription(newPres);


                var appDate = slot.AppointmentDate.ToString("MMM dd, yyyy");  // Example: "Apr 05, 2025"
                var appTime = DateTime.Today.Add(slot.AppointmentTime).ToString("hh:mm tt"); // Example: "10:30 AM"

                var notEntity = new Domain.Entities.Notification
                {
                    Title = "Your Prescription Has Been Sent",
                    Message = $"{doctor.Doctor_name} has shared your prescription for the appointment scheduled on {appDate:MMMM dd, yyyy} at {appTime:hh\\:mm tt}. Please review it at your convenience.",
                    Sender_id = doctor.Id,
                    Sender_Name = doctor.Doctor_name,
                    Sender_Profile = doctor.Profile,
                    Recipient_id = appoinment.PatientId,
                    Recipient_type = "Patient",
                    Created_by = doctor.Doctor_name,
                    Updated_by = doctor.Doctor_name,
                    Created_on = DateTime.UtcNow,
                    Updated_on = DateTime.UtcNow,
                };


                var resNot = new NotificationResDto
                {
                    Sender_Name = notEntity.Sender_Name,
                    Recipient_id = notEntity.Recipient_id,
                    Message = notEntity.Message,
                    Title = notEntity.Title,
                    Sender_Profile = notEntity.Sender_Profile,
                };

                await _notificationService.PrescriptionCreatedNotifyPatient(notEntity, resNot);
                return "Prescription added successfully";

            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
