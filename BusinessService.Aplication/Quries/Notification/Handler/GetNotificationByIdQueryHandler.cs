using BusinessService.Aplication.Common.DTOs.Notification;
using BusinessService.Aplication.Interfaces.IServices;
using Contarcts.Requests.Patient;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BusinessService.Aplication.Quries.Notification.Handler
{
    public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationPageResDto>
    {
        private readonly INotificationService _notificationService;
        private readonly IRequestClient<PatientByIdRequest> _requestClient;
        public GetNotificationByIdQueryHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
           
        }
        public async Task<NotificationPageResDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationBYId(request.authid, request.recipient_type);
                var res = new NotificationPageResDto()
                {
                    total_pages = (int)Math.Ceiling((double)notifications.Count / request.pageSize),
                    items = notifications
                      .OrderBy(n => n.IsRead)
                      .ThenByDescending(n => n.Created_on)
                   .Select(a => new NotificationResApiDto
                   {
                       Id = a.Id,
                       Title = a.Title,
                       Message = a.Message,
                       Sender_Name = a.Sender_Name,
                       Sender_Profile = a.Sender_Profile,
                       IsRead = a.IsRead,
                       Sender_id = a.Sender_id,
                       Created_on = a.Created_on,
                   })
                     .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                   .ToList(),
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
