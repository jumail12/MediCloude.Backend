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

        public async Task<bool> isExists(Guid drid, DateTime date, TimeSpan time)
        {
            try
            {
                var res = await _businessDbContext.DrAvailabilities
                    .FirstOrDefaultAsync(a => a.DrId == drid && a.AppointmentDate == date && a.AppointmentTime == time);

                return res != null;
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
                DateTime today = DateTime.Today; 

                var res = await _businessDbContext.DrAvailabilities
                    .Where(a => a.DrId==drID &&  a.IsAvailable && !a.Is_deleted && a.AppointmentDate>=today)
                    .ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<List<DrAvailability>> GetByDrProfileById(Guid drID, int days)
        {
            try
            {
            
                var latestSlotDate = await _businessDbContext.DrAvailabilities
                    .Where(a => a.DrId == drID && a.IsAvailable && !a.Is_deleted)
                    .MaxAsync(a => (DateTime?)a.AppointmentDate); 

                if (latestSlotDate == null)
                {
                    return new List<DrAvailability>(); 
                }

                if (days == 0)
                {
                    var all = await _businessDbContext.DrAvailabilities
                         .Where(a => a.DrId == drID &&
                         a.IsAvailable &&
                         !a.Is_deleted )
             .ToListAsync();

                    return all;
                }

                DateTime startDate = latestSlotDate.Value.AddDays(-(days - 1)); 

              
                var res = await _businessDbContext.DrAvailabilities
                    .Where(a => a.DrId == drID &&
                                a.IsAvailable &&
                                !a.Is_deleted &&
                                a.AppointmentDate >= startDate &&
                                a.AppointmentDate <= latestSlotDate)
                    .ToListAsync();

                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<DrAvailability> GetBySlotId(Guid slotID)
        {
            try
            {
                var res= await _businessDbContext.DrAvailabilities.FirstOrDefaultAsync(a=>a.Id == slotID && a.IsAvailable && !a.Is_deleted);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

    }
}
