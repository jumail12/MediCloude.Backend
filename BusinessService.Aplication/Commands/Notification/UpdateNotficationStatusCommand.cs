using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.Notification
{
    public record UpdateNotficationStatusCommand(Guid authid,Guid notId,string recip_type) : IRequest<string>;
}
