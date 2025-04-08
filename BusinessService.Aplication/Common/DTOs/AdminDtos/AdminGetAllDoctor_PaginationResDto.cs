using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.AdminDtos
{
    public class AdminGetAllDoctor_PaginationResDto
    {
        public int total_pages { get; set; }
        public List<GetAllDoctor_ResDto> items { get; set; }
    }
    public class GetAllDoctor_ResDto
    {
        public Guid Id { get; set; }
        public string? Doctor_name { get; set; }
        public string? Specialization { get; set; }
        public string? Qualification { get; set; }
        public string? Profile { get; set; }
        public double? Field_experience { get; set; }
        public bool? IsBlocked { get; set; }
    }
}
