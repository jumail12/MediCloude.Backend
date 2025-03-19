using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces.IRepos
{
    public interface IAdminRepo
    {
        Task<List<Admin>> GetAdmins();
        Task<bool> EmailExists(string email);
        Task<bool> AddAdmin(Admin admin);
    }
}
