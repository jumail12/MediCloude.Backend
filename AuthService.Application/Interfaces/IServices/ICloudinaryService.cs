using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadProfileImageAsync(IFormFile file);
    }
}
