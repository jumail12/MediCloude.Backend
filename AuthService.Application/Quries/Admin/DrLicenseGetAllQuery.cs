using AuthService.Application.Common.DTOs.AdminDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Quries.Admin
{
    public record DrLicenseGetAllQuery(int pageNumber, int pageSize) : IRequest<DrLicenseGetAllPageResDto>;
}
