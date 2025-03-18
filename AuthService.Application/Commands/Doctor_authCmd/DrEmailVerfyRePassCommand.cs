using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Doctor_authCmd
{
    public record DrEmailVerfyRePassCommand : IRequest<string>
    {
        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        public int otp { get; set; }
    }
}
