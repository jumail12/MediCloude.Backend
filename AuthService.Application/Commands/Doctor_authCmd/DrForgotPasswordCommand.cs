using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Doctor_authCmd
{
    public  record DrForgotPasswordCommand : IRequest<string>
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
