using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.DTOs.CommonDtos
{
    public class RefreshTokenResDto
    {
        public string? Access_token { get; set; }
        public string? Refresh_token { get; set; }
    }
}
