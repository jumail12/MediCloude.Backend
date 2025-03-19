using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.DTOs.AdminDTOs
{
    public class AdminLoginResDto
    {
        public Guid Id { get; set; } 
        public string Email { get; set; }
        public string? Refresh_token { get; set; }
        public string? Access_token { get; set; }
    }
}
