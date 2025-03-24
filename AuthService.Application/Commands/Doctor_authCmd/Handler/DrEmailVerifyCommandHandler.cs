using AuthService.Application.Commands.Doctor_authCmd;
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
    public class DrEmailVerifyCommandHandler : IRequestHandler<DrEmailVerifyCommand, string>
    {
        private readonly IDrRepo _repo;
        public DrEmailVerifyCommandHandler(IDrRepo drRepo)
        {
            _repo = drRepo;
        }
        public async Task<string> Handle(DrEmailVerifyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var verifiedUsers = await _repo.GetAllVeriFyIdentity();
                var veriUser = verifiedUsers.FirstOrDefault(a => a.Email == request.email && a.Otp == request.otp);

                if (veriUser == null)
                {
                    throw new ValidationException("Not found");
                }

                TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
                if (currentTime <= veriUser.Expire_time)
                {
                    var doctor = new Doctor
                    {
                        Doctor_name = veriUser.Name,
                        Email = veriUser.Email,
                        Password = veriUser.Password,
                        Created_by = veriUser.Name,
                        Created_on = DateTime.UtcNow,
                        Updated_by = veriUser.Name,
                        Updated_on = DateTime.UtcNow,
                        Profile= "http://localhost:3001/src/assets/Images/profile.webp"
                    };
                    await _repo.AddNewVerifiedDr(doctor);
                    await _repo.RemoveVeriFyIdentity(veriUser);
                    return "Email verification success";
                }

                return "Your otp is expired!";

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
