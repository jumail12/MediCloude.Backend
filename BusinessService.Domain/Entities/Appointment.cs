using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.Domain.Entities
{
        public class Appointment : AuditableEntity
        {
            [Key]
            public Guid Id { get; set; }

            [Required(ErrorMessage = "Doctor ID is required")]
            public Guid DrId { get; set; }

            [Required(ErrorMessage = "Doctor Availability ID is required")]
            public Guid DrAvailabilityId { get; set; }

            [Required(ErrorMessage = "Patient ID is required")]
            public Guid PatientId { get; set; }

            [Required(ErrorMessage = "Status is required")]
            public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

            public Guid? RoomId { get; set; } 
        }

        public enum AppointmentStatus
        {
            Pending,
            Confirmed,
            Success,
            Cancelled
        }
    }


