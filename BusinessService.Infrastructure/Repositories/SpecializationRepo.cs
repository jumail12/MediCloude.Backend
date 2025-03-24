using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Domain.Entities;
using BusinessService.Infrastructure.Persistance;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Infrastructure.Repositories
{
    public class SpecializationRepo : ISpecializationRepo
    {
        private readonly BusinessDbContext _context;

        public SpecializationRepo(BusinessDbContext context)
        {
              _context = context;
        }

        public async Task<bool> AddSplAsync(Specialization_doctor specialization_)
        {
            try
            {
                await _context.Specializations.AddAsync(specialization_);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<List<Specialization_doctor>> GetAllSplAsync()
        {
            try
            {
                var res = await _context.Specializations.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Specialization_doctor> GetSplByIdAsync(Guid id)
        {
            try
            {
                var res= await _context.Specializations.FirstOrDefaultAsync(a=>a.Id == id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
