using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Doctor
{
    public  class DrByIdResDto
    {
        public Guid Id { get; set; }

        public string? Doctor_name { get; set; }

        public string Email { get; set; }

        public string? Qualification { get; set; }

        public string Category { get; set; }

        public string? Phone { get; set; }

        public string? About { get; set; }

        public string? Profile { get; set; }

        public string? Gender { get; set; }
        public decimal? Drfee { get; set; } = 500;

        public double? Field_experience { get; set; }
    }
}
