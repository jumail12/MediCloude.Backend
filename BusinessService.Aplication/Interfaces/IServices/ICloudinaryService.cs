using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadProfileImageAsync(IFormFile file);
    }
}
