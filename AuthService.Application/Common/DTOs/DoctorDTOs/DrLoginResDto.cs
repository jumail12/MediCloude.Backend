using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.DTOs.DoctorDTOs
{
    public class DrLoginResDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Doctor_name { get; set; }
        public string Email { get; set; }
        public string? Refresh_token { get; set; }
        public string? Access_token { get; set; }
    }
}
