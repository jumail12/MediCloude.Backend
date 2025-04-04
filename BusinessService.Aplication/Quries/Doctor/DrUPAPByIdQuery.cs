using BusinessService.Aplication.Common.DTOs.Appoinment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Doctor
{
    public record DrUPAPByIdQuery(Guid apId) : IRequest<DrUpcommingAppoinmentsResDto>;
}
