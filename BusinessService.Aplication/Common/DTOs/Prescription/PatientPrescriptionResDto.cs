using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Prescription
{
    public class PatientPrescriptionResDto
    {
        public Guid Id { get; set; }
        public string? PrescriptionText { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? Doctor_name { get; set; }
        public string Email { get; set; }
        public string? Qualification { get; set; }
        public string Category { get; set; }
        public string? Phone { get; set; }
        public string? Profile { get; set; }
        public double? Field_experience { get; set; }
    }
}
