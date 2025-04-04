using BusinessService.Aplication.Interfaces.IServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Notification.Handler
{
    public class NotificationCoundQueryHandler : IRequestHandler<NotificationCoundQuery, int>
    {
        private readonly INotificationService _notservice;
        public NotificationCoundQueryHandler(INotificationService notservice)
        {
            _notservice = notservice;
        }
        public async Task<int> Handle(NotificationCoundQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var notfications = await _notservice.GetNotificationBYId(request.authId, request.recipient_type);
                return notfications
                    .Where(a => !a.IsRead)
                    .Count();
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
