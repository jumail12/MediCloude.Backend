using AuthService.Infrastructure.Persistance;
using Contarcts.Requests.Patient;
using Contarcts.Responses.Patient;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers.Patient
{
    public class GetAllPatientConsumer : IConsumer<GetAllPatientReq>
    {
        private readonly AuthDbContext _authDbContext;
        public GetAllPatientConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<GetAllPatientReq> context)
        {
            try
            {
                var patients= await _authDbContext.Patients.Where(a=>!a.Is_blocked).ToListAsync();

                var res= patients.Select(a=> new PatientByIdResponse
                {
                    Patient_id = a.Patient_id,
                    Patient_name = a.Patient_name,
                    Phone=a.Phone,
                    Address=a.Address,
                    Dob=a.Dob,
                    Email=a.Email,
                    Gender=a.Gender,
                }).ToList();

                await context.RespondAsync(new GetAllPatientResponse { patients = res });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }
    }
}
