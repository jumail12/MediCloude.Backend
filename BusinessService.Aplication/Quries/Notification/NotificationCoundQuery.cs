using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Notification
{
    public record NotificationCoundQuery(Guid authId, string recipient_type) : IRequest<int>;
 
}
