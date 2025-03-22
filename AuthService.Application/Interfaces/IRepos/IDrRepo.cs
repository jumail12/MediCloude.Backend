using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces.IRepos
{
    public interface IDrRepo
    {
        Task<bool> EmailExists(string email);
        Task<bool> IsVerifyIdentityExists(string email);
        Task<bool> AddVeriFyIdentity(VerifyIdentity verifyIdentity);
        Task<List<VerifyIdentity>> GetAllVeriFyIdentity();
        Task<bool> AddNewVerifiedDr(Doctor newVerifiedDR);
        Task<bool> RemoveVeriFyIdentity(VerifyIdentity veriFyIdentity);
        Task<List<Doctor>> GetAllDrs();
        Task<bool> SaveAsync();
        Task<List<Doctor>> GetAllLicenseVeriDrs();
    }
}
