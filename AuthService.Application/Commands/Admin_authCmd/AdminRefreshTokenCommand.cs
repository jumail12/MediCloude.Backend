using AuthService.Application.Common.DTOs.CommonDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Admin_authCmd
{
    public record AdminRefreshTokenCommand : IRequest<RefreshTokenResDto>
    {
        public string? Rtoken { get; set; }
    }
}
