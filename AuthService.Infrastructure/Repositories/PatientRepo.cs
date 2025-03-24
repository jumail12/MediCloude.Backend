using AuthService.Application.Interfaces.IRepos;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Infrastructure.Repositories
{
    public class PatientRepo : IPatientRepo
    {
        private readonly AuthDbContext _authDbContext;

        public PatientRepo(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _authDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            try
            {
                var res=await _authDbContext.Patients.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                var IsExists=await _authDbContext.Patients.AnyAsync(x=> x.Email == email);
                return IsExists ? true : false;
            }
            catch(Exception ex)
            {
                throw  new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> IsVerifyIdentityExists(string email)
        {
            try
            {
                var isEx=await _authDbContext.VerifyIdentitys.FirstOrDefaultAsync(x=> x.Email == email);
                if (isEx != null)
                {
                    _authDbContext.VerifyIdentitys.Remove(isEx);
                    await _authDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> AddVeriFyIdentity(VerifyIdentity verifyIdentity)
        {
            try
            {
                await _authDbContext.VerifyIdentitys.AddAsync(verifyIdentity);
                await _authDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> RemoveVeriFyIdentity(VerifyIdentity veriFyIdentity)
        {
            try
            {
                _authDbContext.VerifyIdentitys.Remove(veriFyIdentity);
                await _authDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<List<VerifyIdentity>> GetAllVeriFyIdentity()
        {
            try
            {
                var res = await _authDbContext.VerifyIdentitys.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> AddNewVerifiedPatient(Patient newVerifiedPatient)
        {
            try
            {
                await _authDbContext.Patients.AddAsync(newVerifiedPatient);
                await _authDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<Patient> GetPatientById(Guid id)
        {
            try
            {
                var res = await _authDbContext.Patients.FirstOrDefaultAsync(a => a.Patient_id == id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }
    }
}
