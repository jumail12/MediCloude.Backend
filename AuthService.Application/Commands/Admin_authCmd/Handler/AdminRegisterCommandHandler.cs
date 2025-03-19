using AuthService.Application.Interfaces.IRepos;
using AuthService.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace AuthService.Application.Commands.Admin_authCmd.Handler
{
    public class AdminRegisterCommandHandler : IRequestHandler<AdminRegisterCommand, string>
    {
        private readonly IAdminRepo _repo;
        public AdminRegisterCommandHandler(IAdminRepo repo)
        {
            _repo = repo;
        }

        public async Task<string> Handle(AdminRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool isEx= await _repo.EmailExists(request.Email);

                if (isEx)
                {
                    throw new ValidationException("This email is alraedy taken");
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

                var newAdmin = new Admin()
                {
                    Email= request.Email,
                    Password=hashPassword,
                    Created_by = "Admin",
                    Created_on = DateTime.UtcNow,
                    Updated_by ="Admin",
                    Updated_on = DateTime.UtcNow
                };

                await _repo.AddAdmin(newAdmin);
                return "Admin registration success";
            }
               catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
