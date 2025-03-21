using BusinessService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IRepos
{
    public interface ISpecializationRepo
    {
        Task<bool> AddSplAsync(Specialization_doctor specialization_);
        Task<List<Specialization_doctor>> GetAllSplAsync();
    }
}
