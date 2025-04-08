using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.AdminDtos
{
    public class AdminPatientByIdResDto
    {
        public Guid Patient_id { get; set; }
        public string? Patient_name { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public int? total_appoinments_taken { get; set; }
        public int? toatal_appoinments_completed { get; set; }
        public int? toatal_appoinments_pending { get; set; }
        public decimal? toatal_spended {  get; set; }
        public decimal? your_profit {  get; set; }
    }
}
