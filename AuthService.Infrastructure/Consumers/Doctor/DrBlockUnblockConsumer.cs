using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistance;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers.Doctor
{
    public class DrBlockUnblockConsumer : IConsumer<DrBlockUnBlockReq>
    {
        private readonly AuthDbContext _authDbContext;
        public DrBlockUnblockConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<DrBlockUnBlockReq> context)
        {
            try
            {
                var doctor= await _authDbContext.Doctors.FirstOrDefaultAsync(a=>a.Id==context.Message.drId);
                if (doctor == null)
                {
                    throw new Exception("doctor not found");
                }
             
                doctor.Is_blocked=!doctor.Is_blocked;
                await _authDbContext.SaveChangesAsync();
                await context.RespondAsync(new DrBlockUnBlockResponse { response = doctor.Is_blocked ? "Blocked" : "Unblocked" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
