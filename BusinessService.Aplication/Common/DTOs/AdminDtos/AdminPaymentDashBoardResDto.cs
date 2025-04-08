using BusinessService.Aplication.Common.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.AdminDtos
{
    public class AdminPaymentDashBoardResDto
    {
        public decimal? profit { get; set; }
        public decimal? sales { get; set; }
        public int? total_appoinments_taken { get; set; }
        public int? toatal_appoinments_completed { get; set; }
        public int? toatal_appoinments_pending { get; set; }
        public int? payment_pending { get; set; }
        public int? payment_failed { get; set; }
        public AdminPaymentDetails_paginationDto payment_deatils { get; set; }
    }

    public class AdminPaymentDetails_paginationDto
    {
        public int total_pages { get; set; }
        public List<AdminPaymentDetailsResDto> items { get; set; }
    }

    public class AdminPaymentDetailsResDto
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? Patient_name { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }
}
