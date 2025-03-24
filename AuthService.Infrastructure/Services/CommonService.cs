using AuthService.Application.Interfaces.IServices;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Services
{
    public class CommonService : ICommonService
    {
        private readonly AuthDbContext _dbcontext;
        public CommonService(AuthDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<bool> AddResfreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                await _dbcontext.RefreshTokens.AddAsync(refreshToken);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message, ex);
            }
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            try
            {
                var res= await _dbcontext.RefreshTokens.FirstOrDefaultAsync(a=>a.Refresh_token == token);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message, ex);
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message, ex);
            }
        }
    }
}
