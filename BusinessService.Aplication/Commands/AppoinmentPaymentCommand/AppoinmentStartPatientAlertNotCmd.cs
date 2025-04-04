using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.AppoinmentPaymentCommand
{
    public record AppoinmentStartPatientAlertNotCmd(Guid appId) : IRequest<string>; 
}
