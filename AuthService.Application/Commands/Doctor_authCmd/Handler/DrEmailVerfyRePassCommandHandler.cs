using AuthService.Application.Interfaces.IRepos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Commands.Doctor_authCmd.Handler
{
    public class DrEmailVerfyRePassCommandHandler : IRequestHandler<DrEmailVerfyRePassCommand, string>
    {
        private readonly IDrRepo _repo;
        public DrEmailVerfyRePassCommandHandler(IDrRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> Handle(DrEmailVerfyRePassCommand request, CancellationToken cancellationToken)
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
                    await _repo.RemoveVeriFyIdentity(veriUser);
                    return "Otp verification completed!";
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
