using AuthService.Application.Interfaces.IServices;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;


namespace AuthService.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var cloudName = Environment.GetEnvironmentVariable("CloudName");
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            var apiSecret = Environment.GetEnvironmentVariable("ApiSecret");

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }


        public async Task<string> UploadProfileImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().AspectRatio("1:1").Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }


    }
}
