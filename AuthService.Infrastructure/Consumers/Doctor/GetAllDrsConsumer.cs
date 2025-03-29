using AuthService.Infrastructure.Persistance;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers.Doctor
{
    public class GetAllDrsConsumer : IConsumer<GetAllDrsReq>
    {
        private readonly AuthDbContext _authDbContext;
        public GetAllDrsConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<GetAllDrsReq> context)
        {
            try
            {
                var drs=  _authDbContext.Doctors.Where(a=>!a.Is_deleted && a.Is_approved).ToList();
                var res= drs.Select(a=> new DrByIdResponse
                {
                    Id = a.Id,
                    Doctor_name = a.Doctor_name,
                    Specialization_id=a.Specialization_id,
                    Email=a.Email,
                    Qualification=a.Qualification,
                    Phone=a.Phone,
                    Profile=a.Profile,
                    About=a.About,
                    Drfee=a.Drfee,
                    Field_experience=a.Field_experience,
                    Gender=a.Gender.ToString(),
                      
                }).ToList();

                await context.RespondAsync(new GetAllDrsResponse() { doctors=res});

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
