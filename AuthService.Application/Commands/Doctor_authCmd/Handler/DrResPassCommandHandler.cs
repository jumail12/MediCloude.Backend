using AuthService.Application.Interfaces.IRepos;
using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Doctor_authCmd.Handler
{
    public class DrResPassCommandHandler : IRequestHandler<DrResPassCommand, string>
    {
        private readonly IDrRepo _repo;
        public DrResPassCommandHandler(IDrRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> Handle(DrResPassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Allpatients = await _repo.GetAllDrs();
                var dr = Allpatients.FirstOrDefault(a => a.Email == request.Email);

                if (dr == null)
                {
                    throw new ValidationException("not found");
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.New_password, salt);

                dr.Password = hashPassword;
                dr.Updated_by = dr.Doctor_name;
                dr.Updated_on = DateTime.UtcNow;

                await _repo.SaveAsync();
                return "New password created";

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
