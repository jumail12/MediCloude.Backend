
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Aplication.Interfaces.IServices;
using Contarcts.Common.DTOs;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Commands.AppoinmentPaymentCommand.Handler
{
    public class AppoinmentStartPatientAlertNotCmdHandler : IRequestHandler<AppoinmentStartPatientAlertNotCmd, string>
    {
        private readonly IAppoinmentRepo _appoinmentRepo;
        private readonly IRequestClient<DrByIdReq> _reqClient;
        private readonly IDrAvailabilityRepo _rAvailabilityRepo;
        private readonly INotificationService _notificationService;
        public AppoinmentStartPatientAlertNotCmdHandler(IAppoinmentRepo appoinmentRepo,IRequestClient<DrByIdReq> requestClient, IDrAvailabilityRepo rAvailabilityRepo,INotificationService notificationService)
        {
            _appoinmentRepo = appoinmentRepo;
            _reqClient = requestClient;
            _rAvailabilityRepo = rAvailabilityRepo;
            _notificationService = notificationService;
        }

        public async Task<string> Handle(AppoinmentStartPatientAlertNotCmd request, CancellationToken cancellationToken)
        {
            try
            {
                var appoinment = await _appoinmentRepo.GetBy_APId(request.appId);
                if (appoinment == null) 
                {
                    throw new Exception("Appoinment not found.");
                }

                var RabbitMqRes = await _reqClient.GetResponse<DrByIdResponse>(new DrByIdReq(appoinment.DrId));

                if (RabbitMqRes == null)
                {
                    throw new Exception("Error making communication with RabbitMQ.");
                }

                var doctor = RabbitMqRes.Message;

                if (doctor == null)
                {
                    throw new Exception("doctor not found.");
                }

                var slot = await _rAvailabilityRepo.GetById(appoinment.DrAvailabilityId);
                if (slot == null)
                {
                    throw new Exception("slot not found.");
                }

                var appDate = slot.AppointmentDate.ToString("MMM dd, yyyy");  // Example: "Apr 05, 2025"
                var appTime = DateTime.Today.Add(slot.AppointmentTime).ToString("hh:mm tt"); // Example: "10:30 AM"

                var notEntity = new Domain.Entities.Notification
                {
                    Title = "Your appointment will start shortly",
                    Message = $"{doctor.Doctor_name} has an appointment with you on {appDate} at {appTime}.",
                    Sender_id = doctor.Id,
                    Sender_Name = doctor.Doctor_name,
                    Sender_Profile=doctor.Profile,
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

                await _notificationService.AppoinmetStartPatientAlert(notEntity, resNot);
                return "Alerted message success";
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
