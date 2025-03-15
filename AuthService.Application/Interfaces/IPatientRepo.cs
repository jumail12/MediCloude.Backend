using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IPatientRepo
    {
        Task<bool> EmailExists(string email);
        Task<List<Patient>> GetAllPatients();
        Task<bool> IsVerifyIdentityExists(string email);
        Task<bool> AddVeriFyIdentity(VerifyIdentity verifyIdentity);
        Task<bool> RemoveVeriFyIdentity(VerifyIdentity veriFyIdentity);
        Task<List<VerifyIdentity>> GetAllVeriFyIdentity();
        Task<bool> AddNewVerifiedPatient(Patient newVerifiedPatient);
        Task<bool> SaveAsync();
    }
}
