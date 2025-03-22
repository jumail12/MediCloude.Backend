using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Admin_authCmd
{
    public class DrLicenseApproveCommand : IRequest<string>
    {
        public Guid DrId { get; set; }
    }
}
