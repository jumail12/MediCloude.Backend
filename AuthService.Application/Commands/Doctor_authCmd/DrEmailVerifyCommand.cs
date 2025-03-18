using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Doctor_authCmd
{
    public record DrEmailVerifyCommand : IRequest<string>
    {
        [EmailAddress]
        public string email { get; set; }
        public int otp { get; set; }
    }
}
