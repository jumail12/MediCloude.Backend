using AuthService.Infrastructure.Persistance;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Contarcts.Common.GenderContarct;

namespace AuthService.Infrastructure.Consumers.Doctor
{
    public class DrProfileUpdationConsumer : IConsumer<DrProfileUpdationReq>
    {
        private readonly AuthDbContext _authDbContext;
        public DrProfileUpdationConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<DrProfileUpdationReq> context)
        {
            try
            {
                var req = context.Message;
                var doctor= await _authDbContext.Doctors.FirstOrDefaultAsync(a=>a.Id==context.Message.drId);
                if(doctor == null)
                {
                    throw new Exception("dr not found");
                }

                doctor.Profile=req.Profile;
                doctor.Phone=req.Phone;
                doctor.About=req.About;
                doctor.Field_experience=req.Field_experience;
                doctor.Qualification=req.Qualification;
                doctor.Updated_by = doctor.Doctor_name;
                doctor.Updated_on = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(req.Gender) && Enum.TryParse<GenderD>(req.Gender, out var parsedGender))
                {
                    doctor.Gender = parsedGender;
                }
                await _authDbContext.SaveChangesAsync();
                await context.RespondAsync(new DrProfileUpdationResponse { Success=true});

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
