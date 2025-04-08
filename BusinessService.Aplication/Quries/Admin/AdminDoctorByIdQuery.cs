using BusinessService.Aplication.Common.DTOs.AdminDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.Admin
{
    public record AdminDoctorByIdQuery(Guid drid) : IRequest<AdminDoctorByIdResDto>;
}
