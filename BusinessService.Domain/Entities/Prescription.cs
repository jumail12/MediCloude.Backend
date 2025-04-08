using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Domain.Entities
{
    public class Prescription : AuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid AppointmentID { get; set; }
        [Required]
        [MaxLength(1000)]
        public string? PrescriptionText { get; set; }
        [Required(ErrorMessage = "Patient ID is required")]
        public Guid PatientId { get; set; }
        [Required(ErrorMessage = "Doctor ID is required")]
        public Guid DrId { get; set; }
    }
}
