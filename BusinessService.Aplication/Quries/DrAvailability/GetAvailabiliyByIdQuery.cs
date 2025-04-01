using BusinessService.Aplication.Common.DTOs.Availability;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.DrAvailability
{
    public record GetAvailabiliyByIdQuery(Guid slotid): IRequest<AvailabilityByIdResDto>;

}
