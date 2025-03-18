using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.DTOs.PatientDTOs
{
    public class PatientLoginResDto
    {
        public Guid Patient_id { get; set; }
        public string Email { get; set; }
        public string? Patient_name { get; set; }
        public string? Refresh_token { get; set; }
        public string? Access_token { get; set; }
    }
}
