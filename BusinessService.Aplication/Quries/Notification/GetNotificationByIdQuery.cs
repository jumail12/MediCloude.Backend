using BusinessService.Aplication.Common.DTOs.Notification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Notification
{
    public record GetNotificationByIdQuery(Guid authid, string recipient_type, int pageNumber, int pageSize) : IRequest<NotificationPageResDto>;
}
