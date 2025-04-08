using BusinessService.Aplication.Common.DTOs.AppoinmentPaymentDtos;
using BusinessService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.CommonContarct;

namespace BusinessService.Aplication.Commands.AppoinmentPaymentCommand
{
    public record PaymentAppoinmentCommand  : IRequest<string>
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "TrasactionId is required")]
        public string TrasactionId { get; set; }

        [Required(ErrorMessage = "Slot ID is required")]
        public Guid DrAvailabilityId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [EnumDataType(typeof(PaymentWay), ErrorMessage = "Invalid payment method")]
        public PaymentWay PaymentMethod { get; set; }

        [Required(ErrorMessage = "Razorpay credentials are required")]
        public PaymentDto RazorPaymentCredential { get; set; }
    }
}
