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
    public class DrAvailabilityRepo : IDrAvailabilityRepo
    {
        private readonly BusinessDbContext _businessDbContext;
        public DrAvailabilityRepo(BusinessDbContext businessDbContext)
        {
            _businessDbContext = businessDbContext;
        }

        public async Task<bool> Add(DrAvailability drAvailability)
        {
            try
            {
                await _businessDbContext.DrAvailabilities.AddAsync(drAvailability);
                await _businessDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<bool> isExists(Guid drid, Day day, TimeSpan time)
        {
            try
            {
                var res= await _businessDbContext.DrAvailabilities.FirstOrDefaultAsync(a=>a.DrId==drid && a.AppointmentDay==day && a.AppointmentTime==time);
                return   res != null ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<List<DrAvailability>> GetByDrId(Guid drID)
        {
            try
            {
                var res = await _businessDbContext.DrAvailabilities
                    .Where(a => a.DrId==drID &&  a.IsAvailable && !a.Is_deleted)
                    .ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
