using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthService.Application.Commands.Doctor_authCmd
{
    public record DrLicenseVerifyCommand : IRequest<string>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization_id { get; set; }

        [Required(ErrorMessage = "License number is required")]
        [RegularExpression(@"^[A-Z]{2,3}/?\d{5,7}/?\d{4}$",
            ErrorMessage = "Invalid Medical License Number format. Example: MH/12345/2020 or TN67890/2015")]
        public string? Medical_license_number { get; set; }
    }
}
