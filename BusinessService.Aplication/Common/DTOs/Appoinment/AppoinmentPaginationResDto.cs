using BusinessService.Aplication.Common.DTOs.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Appoinment
{
    public class AppoinmentPaginationResDto
    {
        public int total_pages { get; set; }
        public List<DrUpcommingAppoinmentsResDto> items { get; set; }

    }
}
