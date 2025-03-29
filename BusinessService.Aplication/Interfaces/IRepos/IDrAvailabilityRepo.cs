using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IRepos
{
    public interface IDrAvailabilityRepo
    {
        Task<bool> Add(DrAvailability drAvailability);
        Task<bool> isExists(Guid drid,DateTime day,TimeSpan time);
        Task<List<DrAvailability>> GetByDrId(Guid drID);
        Task<List<DrAvailability>> GetByDrProfileById(Guid drID,int days);
    }
}
