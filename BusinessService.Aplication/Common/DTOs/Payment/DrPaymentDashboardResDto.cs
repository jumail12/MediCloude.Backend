using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.Payment
{
    public class DrPaymentDashboardResDto
    {
        public decimal? profit { get; set; }
        public int? total_appoinments_taken { get; set; }
        public int? toatal_appoinments_completed { get; set; }
        public int? toatal_appoinments_pending { get; set; }
        public int? payment_pending { get; set; }
        public int? payment_failed { get; set; }
        public DrPaymentDetails_paginationDto payment_deatils { get; set; }
    }

    public class DrPaymentDetails_paginationDto
    {
        public int total_pages { get; set; }
        public List<DrPaymentDetailsResDto> drPayments { get; set; }
    }

    public class DrPaymentDetailsResDto
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? Patient_name { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public decimal DoctorAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }
}
