using AuthService.Infrastructure.Persistance;
using Contarcts.Requests.Doctor;
using Contarcts.Responses.Doctor;
using MassTransit;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Infrastructure.Consumers.Doctor
{
    public class DrByIdConsumer : IConsumer<DrByIdReq>
    {
        private readonly AuthDbContext _authDbContext;
        public DrByIdConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<DrByIdReq> context)
        {
            try
            {
                var doctor= await _authDbContext.Doctors.FirstOrDefaultAsync(a=>a.Id==context.Message.drid);
                if (doctor == null)
                {
                    throw new Exception("doctor not found");
                }
                var res = new DrByIdResponse
                {
                    Id = context.Message.drid,
                    Specialization_id =doctor.Specialization_id,
                    Doctor_name=doctor.Doctor_name,
                    Email=doctor.Email,
                    Qualification=doctor.Qualification,
                    Phone=doctor.Phone,
                    About=doctor.About,
                    Profile=doctor.Profile,
                    Field_experience=doctor.Field_experience,
                    Gender=doctor.Gender.ToString(),
                    Drfee=doctor.Drfee,
                    IsBlocked=doctor.Is_blocked,
                };

                await context.RespondAsync(res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
