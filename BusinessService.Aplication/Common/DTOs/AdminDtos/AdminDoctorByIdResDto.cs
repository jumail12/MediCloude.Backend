using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.AdminDtos
{
    public class AdminDoctorByIdResDto
    {
        public decimal? your_profit { get; set; }
        public int? total_appoinments_taken { get; set; }
        public int? toatal_appoinments_completed { get; set; }
        public int? toatal_appoinments_pending { get; set; }
        public Guid Id { get; set; }
        public string? Doctor_name { get; set; }
        public string? Specialization { get; set; }
        public string Email { get; set; }
        public string? Qualification { get; set; }
        public string? Phone { get; set; }
        public string? Profile { get; set; }
        public string? Gender { get; set; }
        public double? Field_experience { get; set; }
        public bool IsBlocked { get; set; }
    }
}
