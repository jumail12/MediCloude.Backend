using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Doctor
{
    public class GetAllDrsPageResDto
    {
        public int total_pages { get; set; }
        public List<GetAllDrsResDto> doctors { get; set; }
    }
}
