using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.DTOs.AdminDTOs
{
    public class DrLicenseGetAllPageResDto
    {
        public int total_pages { get; set; }
         public List<DrLicenseGetAllResDto> items { get; set; }
    }
}
