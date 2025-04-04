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
    public class AppoinmentRepo : IAppoinmentRepo
    {
        private readonly  BusinessDbContext _businessDbContext;
        public AppoinmentRepo(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task<Guid> AddAppoinment(Appointment appointment)
        {
            try
            {
                await _businessDbContext.Appointments.AddAsync(appointment);
                await _businessDbContext.SaveChangesAsync();
                return appointment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<List<Appointment>> GetAppoinmentListByDrId(Guid drid)
        {
            try
            {
                var res = await _businessDbContext.Appointments.Where(a => !a.Is_deleted && a.DrId == drid && a.Status.ToString()== "Confirmed").ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<Appointment> GetBy_APId(Guid id)
        {
            try
            {
                var res = await _businessDbContext.Appointments.FirstOrDefaultAsync(a=>a.Id == id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<List<Appointment>> GetByPatientID(Guid patientID)
        {
            try
            {
                var res = await _businessDbContext.Appointments.Where(a=>!a.Is_deleted && a.PatientId == patientID && a.Status.ToString()== "Confirmed").ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
