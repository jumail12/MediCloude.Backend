using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.Domain.Entities
{
    public class DrAvailability : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Dr id is required")]
        public Guid DrId { get; set; }

        [Required(ErrorMessage = "Appointment day is required")]
        public Day AppointmentDay { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [Column(TypeName = "time")]
        public TimeSpan AppointmentTime { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public enum Day
    {
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
    }
}
