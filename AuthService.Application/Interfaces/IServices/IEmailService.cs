using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<bool> SendOtp(string email, int otp);
        Task<bool> DrLicenseApprovedEmail(string email);
        Task<bool> DrLicenseRejectedEmail(string email);
    }
}
