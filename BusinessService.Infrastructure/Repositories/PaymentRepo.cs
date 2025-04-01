using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using BusinessService.Infrastructure.Persistance;
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
    }
}
