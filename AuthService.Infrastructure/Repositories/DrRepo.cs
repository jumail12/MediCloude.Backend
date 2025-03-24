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

    public class DrRepo : IDrRepo
    {
        private readonly AuthDbContext _authDb;
        public DrRepo(AuthDbContext authDb)
        {
            _authDb = authDb;
        }
        public async Task<bool> EmailExists(string email)
        {
            try
            {
                bool isExists= await _authDb.Doctors.AnyAsync(a=>a.Email == email);
                return isExists;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> IsVerifyIdentityExists(string email)
        {
            try
            {
                var isEx = await _authDb.VerifyIdentitys.FirstOrDefaultAsync(x => x.Email == email);
                if (isEx != null)
                {
                    _authDb.VerifyIdentitys.Remove(isEx);
                    await _authDb.SaveChangesAsync();
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
                await _authDb.VerifyIdentitys.AddAsync(verifyIdentity);
                await _authDb.SaveChangesAsync();
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
                var res= await _authDb.VerifyIdentitys.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> AddNewVerifiedDr(Doctor newVerifiedDR)
        {
            try
            {
                await _authDb.Doctors.AddAsync(newVerifiedDR);
                await _authDb.SaveChangesAsync();
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
                 _authDb.VerifyIdentitys.Remove(veriFyIdentity);
                await _authDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<List<Doctor>> GetAllDrs()
        {
            try
            {
                var res = await _authDb.Doctors.ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _authDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<List<Doctor>> GetAllLicenseVeriDrs()
        {
            try
            {
                var res= await _authDb.Doctors.Where(a=>!a.Is_approved && !a.Is_blocked && !a.Is_deleted).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }

        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            try
            {
                var res= await _authDb.Doctors.FirstOrDefaultAsync(a=>a.Id == id);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.ToString());
            }
        }
    }
}
