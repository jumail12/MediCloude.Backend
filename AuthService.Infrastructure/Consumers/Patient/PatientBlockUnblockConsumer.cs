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
    public class PatientBlockUnblockConsumer : IConsumer<PatientBlockUnblockReq>
    {
        private readonly AuthDbContext _authDbContext;
        public PatientBlockUnblockConsumer(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task Consume(ConsumeContext<PatientBlockUnblockReq> context)
        {
            try
            {
                var patient = await _authDbContext.Patients.FirstOrDefaultAsync(a=>a.Patient_id==context.Message.patientid);
                if (patient == null)
                {
                    throw new Exception("Patient not found");
                }

               
                patient.Is_blocked = !patient.Is_blocked;
                await _authDbContext.SaveChangesAsync();
                await context.RespondAsync(new PatientBlockUnblockResponse {response=patient.Is_blocked? "Blocked" : "Unblocked" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }
    }
}
