using BusinessService.Aplication.Common.DTOs.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Payment
{
    public record DrPaymentDashboardQuery(Guid drid, int pageNumber, int pageSize) : IRequest<DrPaymentDashboardResDto>;

}
