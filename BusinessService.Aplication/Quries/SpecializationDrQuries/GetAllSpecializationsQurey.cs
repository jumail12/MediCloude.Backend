using BusinessService.Aplication.Common.DTOs.SpecializationDtos;
using BusinessService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.SpecializationDrQuries
{
    public record GetAllSpecializationsQurey() : IRequest<List<GetAllSpecializationResDto>>;

}
