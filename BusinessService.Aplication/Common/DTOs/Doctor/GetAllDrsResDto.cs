using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Doctor
{
    public class GetAllDrsResDto
    {
        public Guid Id { get; set; }

        public string? Doctor_name { get; set; }
        public string? Qualification { get; set; }
        public string Category { get; set; }
        public string? Profile { get; set; }
        public decimal? Drfee { get; set; } = 500;
        public double? Field_experience { get; set; }
    }
}
