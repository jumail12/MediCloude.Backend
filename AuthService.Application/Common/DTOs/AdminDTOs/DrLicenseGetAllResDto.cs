using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.DTOs.AdminDTOs
{
    public class DrLicenseGetAllResDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Doctor_name { get; set; }
        public string Email { get; set; }
        public  string? Specialization { get; set; }
        public string? Medical_license_number { get; set; }
    }
}
