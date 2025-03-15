using MediatR;
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.Commands.Patient_authCmd
{
    public record PatientEmailVerifyCommand : IRequest<string>
    {
        [EmailAddress]
        public string email { get; set; }
        public int otp{ get; set; }
    }
}
