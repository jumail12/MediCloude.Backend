using AuthService.Application.Common.DTOs.CommonDtos;
using MediatR;


namespace AuthService.Application.Commands.Doctor_authCmd
{
    public record DrRefreshTokenCommand : IRequest<RefreshTokenResDto>
    {
        public string? Rtoken { get; set; }
    }
}
