using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using BusinessService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Infrastructure.Repositories
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly BusinessDbContext _businessDbContext;
        public PaymentRepo(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task<bool> AddPaymentDetails(Payment payment)
        {
            try
            {
                await _businessDbContext.Payments.AddAsync(payment);
                await _businessDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<List<Payment>>? GetAllPaymentDetailsByDrId(Guid drid)
        {
            try
            {
                var res = await _businessDbContext.Payments.Where(a => a.DoctorId == drid).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<List<Payment>> GetAllPaymentsByPatientId(Guid patientId)
        {
            try
            {
                var res = await _businessDbContext.Payments.Where(a => a.PatientId == patientId).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<List<Payment>> GetALlPayments()
        {
            try
            {
                var res = await _businessDbContext.Payments.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
