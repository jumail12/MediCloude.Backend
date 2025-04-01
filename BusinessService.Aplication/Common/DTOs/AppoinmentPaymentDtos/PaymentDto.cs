using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Common.DTOs.AppoinmentPaymentDtos
{
    public class PaymentDto
    {
        [Required(ErrorMessage = "payment_id is required")]
        public string? razorpay_payment_id { get; set; }
        [Required(ErrorMessage = "order_id is required")]
        public string? razorpay_order_id { get; set; }
        [Required(ErrorMessage = "Signature is required")]
        public string? razorpay_signature { get; set; }
    }
}
