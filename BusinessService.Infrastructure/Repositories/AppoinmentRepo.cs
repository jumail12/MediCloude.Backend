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
    }
}
