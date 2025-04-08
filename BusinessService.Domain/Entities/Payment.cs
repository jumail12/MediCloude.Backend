using System;
using System.ComponentModel.DataAnnotations;
using static Contarcts.Common.CommonContarct;

namespace BusinessService.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Appointment ID is required")]
        public Guid AppointmentId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Transaction ID is required")]
        [MaxLength(50, ErrorMessage = "Transaction ID cannot exceed 50 characters")]
        public string TransactionId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Doctor amount must be a positive value")]
        public decimal DoctorAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Admin commission amount must be a positive value")]
        public decimal AdminCommissionAmount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [MaxLength(20)]
        public PaymentWay PaymentMethod { get; set; }  

        [Required(ErrorMessage = "Payment status is required")]
        public Status PaymentStatus { get; set; }
    }

 
}
