using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces.IServices
{
    public interface ICommonService
    {
        Task<bool> AddResfreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task<bool> SaveAsync();
    }
}
