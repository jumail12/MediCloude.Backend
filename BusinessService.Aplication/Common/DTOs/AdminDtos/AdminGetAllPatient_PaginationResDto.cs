using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.AdminDtos
{
    public class AdminGetAllPatient_PaginationResDto
    {
        public int total_pages { get; set; }
        public List<GetAllPatient_ResDto> items { get; set; }
    }

    public class GetAllPatient_ResDto 
    {
        public Guid Patient_id { get; set; }

        public string? Patient_name { get; set; }

        public string Email { get; set; }
        public bool IsBlocked { get; set; }
    }


}
