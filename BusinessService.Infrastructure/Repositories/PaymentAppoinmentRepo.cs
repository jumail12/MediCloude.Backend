using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using BusinessService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using BusinessService.Aplication.Common.DTOs.AppoinmentPaymentDtos;
using Razorpay.Api;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Infrastructure.Repositories
{
    public class PaymentAppoinmentRepo : IPaymentAppoinmentRepo
    {
        private readonly BusinessDbContext _businessDbContext;
        public PaymentAppoinmentRepo(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task<bool> PaymentAppoinmentGateway(Appointment appointment, Domain.Entities.Payment payment, PaymentDto paymentDto)
        {
            var strategy = _businessDbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _businessDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _businessDbContext.Appointments.AddAsync(appointment);
                        var slot = await _businessDbContext.DrAvailabilities.FirstOrDefaultAsync(a => a.Id == appointment.DrAvailabilityId);

                        if (slot == null) 
                        {
                            throw new ValidationException("Appointment slot is not found");
                        }

                        slot.IsAvailable = false;
                        await _businessDbContext.SaveChangesAsync();

                        payment.AppointmentId = appointment.Id;
                        await _businessDbContext.Payments.AddAsync(payment);
                        await _businessDbContext.SaveChangesAsync();

                        bool isPaymentValid = await RazorPaymentVerify(paymentDto);
                        if (!isPaymentValid)
                        {
                            throw new Exception("Payment verification failed.");
                        }

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (ValidationException)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(ex.InnerException?.Message ?? ex.Message);
                    }
                }
            });
        }


        private async Task<bool> RazorPaymentVerify(PaymentDto payment)
        {
            if (payment == null ||
               string.IsNullOrEmpty(payment.razorpay_payment_id) ||
               string.IsNullOrEmpty(payment.razorpay_order_id) ||
               string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return false;
            }

            try
            {
                RazorpayClient client = new RazorpayClient(
                   Environment.GetEnvironmentVariable("RazorpayKeyId"),
                   Environment.GetEnvironmentVariable("RazorpayKeySecret")
                );

                Dictionary<string, string> attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", payment.razorpay_payment_id },
            { "razorpay_order_id", payment.razorpay_order_id },
            { "razorpay_signature", payment.razorpay_signature }
        };

                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in RazorPayment Verification: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
