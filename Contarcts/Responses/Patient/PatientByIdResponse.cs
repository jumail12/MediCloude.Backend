using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.GenderContarct;

namespace Contarcts.Responses.Patient
{
    public class PatientByIdResponse
    {
        public Guid Patient_id { get; set; }

        public string? Patient_name { get; set; }

        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string? Phone { get; set; }

        public string? Address { get; set; }
        public GenderP? Gender { get; set; }
    }
}
