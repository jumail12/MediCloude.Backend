using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IRepos
{
    public interface IPaymentRepo
    {
        Task<bool> AddPaymentDetails(Payment payment);
        Task<List<Payment>> GetAllPaymentDetailsByDrId(Guid drid);
        Task<List<Payment>> GetAllPaymentsByPatientId(Guid patientId);
        Task<List<Payment>> GetALlPayments();
    }
}
