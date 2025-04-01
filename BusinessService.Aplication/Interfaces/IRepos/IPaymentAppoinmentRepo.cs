using BusinessService.Aplication.Common.DTOs.AppoinmentPaymentDtos;
using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IRepos
{
    public interface IPaymentAppoinmentRepo
    {
        Task<bool> PaymentAppoinmentGateway(Appointment appointment,Payment payment,PaymentDto paymentDto);
    }
}
