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
    public class PrescriptionRepo : IPrescriptionRepo
    {
        private readonly BusinessDbContext _businessDbContext;
        public PrescriptionRepo(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task<bool> AddPrescription(Prescription prescription)
        {
            try
            {
                await _businessDbContext.Prescriptions.AddAsync(prescription);
                await _businessDbContext.SaveChangesAsync();    
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception (ex.InnerException?.Message ?? ex.ToString ());
            }
        }

        public async Task<List<Prescription>> GetPrescriptionList_Patient(Guid patientid)
        {
            try
            {
                var res = await _businessDbContext.Prescriptions.Where(a => a.PatientId == patientid).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

    }
}
