using AuthService.Application.Interfaces.IRepos;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class AdminRepo : IAdminRepo
    { 
        private readonly AuthDbContext _authDbContext;
        public  AdminRepo(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task<List<Admin>> GetAdmins()
        {
            try
            {
                var res = await _authDbContext.Admins.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                bool isEx= await _authDbContext.Admins.AnyAsync(a=>a.Email == email);
                return isEx;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<bool> AddAdmin(Admin admin)
        {
            try
            {
                await _authDbContext.Admins.AddAsync(admin);
                await _authDbContext.SaveChangesAsync();    
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Admin> GetByIdAsync(Guid id)
        {
            try
            {
                var res = await _authDbContext.Admins.FirstOrDefaultAsync(a=>a.Id==id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
