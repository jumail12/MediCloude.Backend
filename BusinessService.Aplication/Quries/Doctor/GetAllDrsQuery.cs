using BusinessService.Aplication.Common.DTOs.Doctor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Doctor
{
    public record GetAllDrsQuery(int pageNumber, int pageSize) : IRequest<GetAllDrsPageResDto>;
}
