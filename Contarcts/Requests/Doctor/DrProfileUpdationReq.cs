using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.GenderContarct;

namespace Contarcts.Requests.Doctor
{
    public record DrProfileUpdationReq
    {

        public Guid drId { get; set; }
        public string? Phone { get; set; }
        public string? About { get; set; }
        public string? Profile { get; set; }
        public string? Gender { get; set; } 
        public double? Field_experience { get; set; }
        public string? Qualification { get; set; }

    }
}
