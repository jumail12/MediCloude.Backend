using AuthService.Application.Common.DTOs.CommonDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Patient_authCmd
{
    public record PatientRefreshTokenCommand : IRequest<RefreshTokenResDto>
    {
        public string? Rtoken {  get; set; }
    }
}
